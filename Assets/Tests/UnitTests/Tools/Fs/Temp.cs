using System;
using System.IO;

namespace Tests.UnitTests.Tools.Fs
{
    
    /// <summary>
    /// Creates a temporal object in OS current temporal storage.
    ///
    /// You can create these types of objects:
    /// <list type="bullet">
    ///     <item>
    ///         <term>File: </term>
    ///         <description>Create a temporal file in temporal storage.</description>
    ///     </item>
    ///     <item>
    ///         <term>Folder: </term>
    ///         <description>Create a temporal folder in temporal storage.</description>
    ///     </item>
    /// </list>
    ///
    /// This class is supposed to be used in an <c>using</c> clause:
    /// <example>
    ///     <code> 
    ///         using (var tempFolder = new Temp(Temp.TempType.Folder)
    ///         {
    ///             // Operate with tempFolder.
    ///         } // tempFolder and its contents is removed when using statement ends.
    ///     </code>
    /// </example>
    /// <exception cref="IOException">
    /// An I/O error occurs, such as no unique temporary name is available. -or-
    /// This method was unable to create a temporary file or folder.
    /// </exception>
    /// </summary>
    public class Temp: IDisposable
    {
        /// <summary>
        /// You can generate two types of temporal object: File and Folder.
        /// </summary>
        public enum TempType
        {
            /// <summary>
            /// Create a temporal file at default temporal folder.
            ///
            /// File name is random.
            /// </summary>
            File, 
            
            /// <summary>
            ///  Create a temporal folder at default temporal folder.
            ///
            /// Folder name is random.
            /// </summary>
            Folder
        }

        private TempType _type;
        private string _tempPath;
    
        /// <summary>
        /// Create a temporal object at system default temp folder.
        /// </summary>
        /// <param name="type">Kind of temporal folder to be created.</param>
        public Temp(TempType type)
        {
            _type = type;
            switch (_type)
            {
                case TempType.File:
                    _tempPath = Path.GetTempFileName();
                    break;
                case TempType.Folder:
                    _tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(_tempPath);
                    break;
            }
        }

        /// <summary>
        /// Kind of this temporal generated object.
        /// </summary>
        public TempType Type => _type;

        /// <summary>
        /// Path to this generated temporal object.
        /// </summary>
        public string TempPath => _tempPath;

        /// <summary>
        /// On dispose, this context manager removes generated temporary object. 
        /// </summary>
        public void Dispose()
        {
            switch (_type)
            {
                case TempType.File:
                    File.Delete(_tempPath);
                    break;
                case TempType.Folder:
                    Directory.Delete(_tempPath, true);
                    break;
            }
        }
    }
}