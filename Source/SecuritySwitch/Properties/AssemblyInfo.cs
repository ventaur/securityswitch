// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: AssemblyTitle("Security Switch")]

[assembly: CLSCompliant(true)]
[assembly: AllowPartiallyTrustedCallers]

[assembly: InternalsVisibleTo("SecuritySwitch.Tests")]