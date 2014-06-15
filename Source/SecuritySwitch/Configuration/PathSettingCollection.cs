// =================================================================================
// Copyright © 2004 Matt Sollars
// All rights reserved.
// 
// This code and information is provided "as is" without warranty of any kind,
// either expressed or implied, including, but not limited to, the implied 
// warranties of merchantability and/or fitness for a particular purpose.
// =================================================================================
using System.Configuration;


namespace SecuritySwitch.Configuration {
	/// <summary>
	/// Represents a collection of PathSetting objects.
	/// </summary>
	public class PathSettingCollection : ConfigurationElementCollection {
		#region Properties
		
		/// <summary>
		/// Gets the PathSetting at the specified index.
		/// </summary>
		/// <param name="index">The index to retrieve the element from.</param>
		/// <returns>The PathSetting located at the specified index.</returns>
		public PathSetting this[int index] {
			get { return (PathSetting)BaseGet(index); }
		}

		/// <summary>
		/// Gets the element name for this collection.
		/// </summary>
		protected override string ElementName {
			get { return "paths"; }
		}

		/// <summary>
		/// Gets a flag indicating an exception should be thrown if a duplicate element 
		/// is added to the collection.
		/// </summary>
		protected override bool ThrowOnDuplicate {
			get { return true; }
		}

		#endregion

		/// <summary>
		/// Adds the specified path setting.
		/// </summary>
		/// <param name="pathSetting">The path setting to add.</param>
		public void Add(PathSetting pathSetting) {
			BaseAdd(pathSetting);
		}

		/// <summary>
		/// Inserts the specified path setting at the index.
		/// </summary>
		/// <param name="index">The index location at which to insert the path setting.</param>
		/// <param name="pathSetting">The path setting to insert.</param>
		public void Insert(int index, PathSetting pathSetting) {
			BaseAdd(index, pathSetting);
		}

		/// <summary>
		/// Returns the index of a specified path setting in the collection.
		/// </summary>
		/// <param name="pathSetting">The path setting to find.</param>
		/// <returns>Returns the index of the path setting.</returns>
		public int IndexOf(PathSetting pathSetting) {
			return (pathSetting != null ? BaseIndexOf(pathSetting) : -1);
		}


		/// <summary>
		/// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
		/// </summary>
		/// <returns>
		/// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
		/// </returns>
		protected override ConfigurationElement CreateNewElement() {
			return new PathSetting();
		}

		/// <summary>
		/// Gets the element key for a specified configuration element when overridden in a derived class.
		/// </summary>
		/// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for.</param>
		/// <returns>
		/// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
		/// </returns>
		protected override object GetElementKey(ConfigurationElement element) {
			return ((PathSetting)element).Path;
		}
	}
}