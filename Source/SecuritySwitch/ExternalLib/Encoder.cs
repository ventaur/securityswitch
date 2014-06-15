// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Encoder.cs" company="Microsoft Corporation">
//   Copyright (c) 2008, 2009, 2010 All Rights Reserved, Microsoft Corporation
//
//   This source is subject to the Microsoft Permissive License.
//   Please see the License.txt file for more information.
//   All other rights reserved.
//
//   THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
//   KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//   IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//   PARTICULAR PURPOSE.
//
// </copyright>
// <summary>
//   Performs encoding of input strings to provide protection against
//   Cross-Site Scripting (XSS) attacks and LDAP injection attacks in
//   various contexts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// NOTE: Some portions of this class were removed, as they were unnecessary.

namespace Microsoft.Security.Application
{
    using System.Globalization;
    
    /// <summary>
    /// Performs encoding of input strings to provide protection against
    /// Cross-Site Scripting (XSS) attacks and LDAP injection attacks in 
    /// various contexts.
    /// </summary>
    /// <remarks>
    /// This encoding library uses the Principle of Inclusions, 
    /// sometimes referred to as "safe-listing" to provide protection 
    /// against injection attacks.  With safe-listing protection, 
    /// algorithms look for valid inputs and automatically treat 
    /// everything outside that set as a potential attack.  This library 
    /// can be used as a defense in depth approach with other mitigation 
    /// techniques. It is suitable for applications with high security 
    /// requirements.
    /// </remarks>
    public static class Encoder
    {
        /// <summary>
        /// Empty string for Java Script context
        /// </summary>
        private const string JavaScriptEmptyString = "''";

        /// <summary>
        /// Initializes character Html encoding array
        /// </summary>
        private static readonly char[][] SafeListCodes = InitializeSafeList();

        /// <summary>
        /// Encodes input strings for use in JavaScript.
        /// </summary>
        /// <param name="input">String to be encoded.</param>
        /// <returns>
        /// Encoded string for use in JavaScript.
        /// </returns>
        /// <remarks>
        /// This function encodes all but known safe characters.  Characters are encoded using \xSINGLE_BYTE_HEX and \uDOUBLE_BYTE_HEX notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Space</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>'alert\x28\x27XSS Attack\x21\x27\x29\x3b'</description></item>
        /// <item><term>user@contoso.com</term><description>'user\x40contoso.com'</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>'Anti-Cross Site Scripting Library'</description></item>
        /// </list>
        /// </remarks>
        public static string JavaScriptEncode(string input)
        {
            return JavaScriptEncode(input, true);
        }

        /// <summary>
        /// Encodes input strings for use in JavaScript.
        /// </summary>
        /// <param name="input">String to be encoded.</param>
        /// <param name="emitQuotes">value indicating whether or not to emit quotes. true = emit quote. false = no quote.</param>
        /// <returns>
        /// Encoded string for use in JavaScript and does not return the output with en quotes.
        /// </returns>
        /// <remarks>
        /// This function encodes all but known safe characters.  Characters are encoded using \xSINGLE_BYTE_HEX and \uDOUBLE_BYTE_HEX notation.
        /// <newpara/>
        /// Safe characters include:
        /// <list type="table">
        /// <item><term>a-z</term><description>Lower case alphabet</description></item>
        /// <item><term>A-Z</term><description>Upper case alphabet</description></item>
        /// <item><term>0-9</term><description>Numbers</description></item>
        /// <item><term>,</term><description>Comma</description></item>
        /// <item><term>.</term><description>Period</description></item>
        /// <item><term>-</term><description>Dash</description></item>
        /// <item><term>_</term><description>Underscore</description></item>
        /// <item><term> </term><description>Space</description></item>
        /// <item><term> </term><description>Other International character ranges</description></item>
        /// </list>
        /// <newpara/>
        /// Example inputs and encoded outputs:
        /// <list type="table">
        /// <item><term>alert('XSS Attack!');</term><description>'alert\x28\x27XSS Attack\x21\x27\x29\x3b'</description></item>
        /// <item><term>user@contoso.com</term><description>'user\x40contoso.com'</description></item>
        /// <item><term>Anti-Cross Site Scripting Library</term><description>'Anti-Cross Site Scripting Library'</description></item>
        /// </list>
        /// </remarks>
        public static string JavaScriptEncode(string input, bool emitQuotes)
        {
            // Input validation: empty or null string condition
            if (string.IsNullOrEmpty(input))
            {
                return emitQuotes ? JavaScriptEmptyString : string.Empty;
            }

            // Use a new char array.
            int outputLength = 0;
            int inputLength = input.Length;
            char[] returnMe = new char[inputLength * 8]; // worst case length scenario

            // First step is to start the encoding with an apostrophe if flag is true.
            if (emitQuotes)
            {
                returnMe[outputLength++] = '\'';
            }

            for (int i = 0; i < inputLength; i++)
            {
                int currentCharacterAsInteger = input[i];
                char currentCharacter = input[i];
                if (SafeListCodes[currentCharacterAsInteger] != null || currentCharacterAsInteger == 92 || (currentCharacterAsInteger >= 123 && currentCharacterAsInteger <= 127))
                {
                    // character needs to be encoded
                    if (currentCharacterAsInteger >= 127)
                    {
                        returnMe[outputLength++] = '\\';
                        returnMe[outputLength++] = 'u';
                        string hex = ((int)currentCharacter).ToString("x", CultureInfo.InvariantCulture).PadLeft(4, '0');
                        returnMe[outputLength++] = hex[0];
                        returnMe[outputLength++] = hex[1];
                        returnMe[outputLength++] = hex[2];
                        returnMe[outputLength++] = hex[3];
                    }
                    else
                    {
                        returnMe[outputLength++] = '\\';
                        returnMe[outputLength++] = 'x';
                        string hex = ((int)currentCharacter).ToString("x", CultureInfo.InvariantCulture).PadLeft(2, '0');
                        returnMe[outputLength++] = hex[0];
                        returnMe[outputLength++] = hex[1];
                    }
                }
                else
                {
                    // character does not need encoding
                    returnMe[outputLength++] = input[i];
                }
            }

            // Last step is to end the encoding with an apostrophe if flag is true.
            if (emitQuotes)
            {
                returnMe[outputLength++] = '\'';
            }

            return new string(returnMe, 0, outputLength);
        }

        /// <summary>
        /// Initializes the safe list.
        /// </summary>
        /// <returns>A two dimensional character array containing characters and their encoded values.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "This is necessary complexity.")]
        private static char[][] InitializeSafeList()
        {
            char[][] allCharacters = new char[65536][];
            for (int i = 0; i < allCharacters.Length; i++)
            {
                if (
                    (i >= 97 && i <= 122) ||        // a-z
                    (i >= 65 && i <= 90) ||         // A-Z
                    (i >= 48 && i <= 57) ||         // 0-9
                    i == 32 ||                      // space
                    i == 46 ||                      // .
                    i == 44 ||                      // ,
                    i == 45 ||                      // -
                    i == 95 ||                      // _
                    (i >= 256 && i <= 591) ||       // Latin,Extended-A,Latin Extended-B        
                    (i >= 880 && i <= 2047) ||      // Greek and Coptic,Cyrillic,Cyrillic Supplement,Armenian,Hebrew,Arabic,Syriac,Arabic,Supplement,Thaana,NKo
                    (i >= 2304 && i <= 6319) ||     // Devanagari,Bengali,Gurmukhi,Gujarati,Oriya,Tamil,Telugu,Kannada,Malayalam,Sinhala,Thai,Lao,Tibetan,Myanmar,eorgian,Hangul Jamo,Ethiopic,Ethiopic Supplement,Cherokee,Unified Canadian Aboriginal Syllabics,Ogham,Runic,Tagalog,Hanunoo,Buhid,Tagbanwa,Khmer,Mongolian   
                    (i >= 6400 && i <= 6687) ||     // Limbu, Tai Le, New Tai Lue, Khmer, Symbols, Buginese
                    (i >= 6912 && i <= 7039) ||     // Balinese         
                    (i >= 7680 && i <= 8191) ||     // Latin Extended Additional, Greek Extended        
                    (i >= 11264 && i <= 11743) ||   // Glagolitic, Latin Extended-C, Coptic, Georgian Supplement, Tifinagh, Ethiopic Extended    
                    (i >= 12352 && i <= 12591) ||   // Hiragana, Katakana, Bopomofo       
                    (i >= 12688 && i <= 12735) ||   // Kanbun, Bopomofo Extended        
                    (i >= 12784 && i <= 12799) ||   // Katakana, Phonetic Extensions         
                    (i >= 19968 && i <= 40899) ||   // Mixed japanese/chinese/korean
                    (i >= 40960 && i <= 42191) ||   // Yi Syllables, Yi Radicals        
                    (i >= 42784 && i <= 43055) ||   // Latin Extended-D, Syloti, Nagri        
                    (i >= 43072 && i <= 43135) ||   // Phags-pa         
                    (i >= 44032 && i <= 55215) /* Hangul Syllables */)
                {
                    allCharacters[i] = null;
                }
                else
                {
                    string integerStringValue = i.ToString(CultureInfo.InvariantCulture);
                    int integerStringLength = integerStringValue.Length;
                    char[] thisChar = new char[integerStringLength];
                    for (int j = 0; j < integerStringLength; j++)
                    {
                        thisChar[j] = integerStringValue[j];
                    }

                    allCharacters[i] = thisChar;
                }
            }

            return allCharacters;
        }
    }
}