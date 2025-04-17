using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    /// <summary>
    /// Every line in build report is parsed to one of this structs.
    /// </summary>
    public struct BuildReportEntry
    {
        public readonly string Size;
        public readonly string SizeUnit;
        public readonly string Percentage;

        public BuildReportEntry(string size, string percentage, string sizeUnit)
        {
            Size = size;
            Percentage = percentage;
            SizeUnit = sizeUnit;
        }
    }
    public class BuildSizesReport : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset = default;

        private Label _texturesSizeLabel, 
            _meshesSizeLabel, 
            _animationsSizeLabel, 
            _soundsSizeLabel, 
            _shadersSizeLabel, 
            _othersAssetsSizeLabel, 
            _levelsSizeLabel, 
            _scriptsSizeLabel, 
            _includedDLLsSizeLabel, 
            _fileHeadersSizeLabel, 
            _totalUserAssetsSizeLabel, 
            _completeBuildSizeLabel;

        private Label _texturesPercentageLabel,
            _meshesPercentageLabel,
            _animationsPercentageLabel,
            _soundsPercentageLabel,
            _shadersPercentageLabel,
            _othersAssetsPercentageLabel,
            _levelsPercentageLabel,
            _scriptsPercentageLabel,
            _includedDLLsPercentageLabel,
            _fileHeadersPercentageLabel,
            _totalUserAssetsPercentageLabel;

        private const string BuildReportTag = "Build Report";
        private const string UncompressedTag = "Uncompressed";
        private const string TexturesTag = "Textures";
        private const string MeshesTag = "Meshes";
        private const string AnimationsTag = "Animations";
        private const string SoundsTag = "Sounds";
        private const string ShadersTag = "Shaders";
        private const string OtherAssetsTag = "Other Assets";
        private const string LevelsTag = "Levels";
        private const string ScriptsTag = "Scripts";
        private const string IncludedDLLsTag = "Included DLLs";
        private const string FileHeadersTag = "File headers";
        private const string TotalUserAssetsTag = "Total User Assets";
        private const string CompleteBuildSizeTag = "Complete build size";
        
        private static Regex rx = new Regex(@"(\d+\.\d+)\s+([a-z]{2})\s*(\d+\.\d+%)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        private Label _lastRefreshLabel;
        private Button _refreshButton;
        private string _logPath;

        private VisualElement _root;
        
        [MenuItem("Tools/Build sizes report")]
        private static void ShowWindow()
        {
            var window = GetWindow<BuildSizesReport>();
            window.titleContent = new GUIContent("Last build sizes report");
            window.Show();
        }

        private void Awake()
        {
            _logPath = Path.Combine(new string[]{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Unity", "Editor", "Editor.log"});
        }

        private void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            _root = rootVisualElement;
            
            // Instantiate UXML
            VisualElement uiXML = visualTreeAsset.Instantiate();
            _root.Add(uiXML);
            
            // Get references to all UI elements.
            GetSizeLabels();
            GetPercentageLabels();
            _lastRefreshLabel = _root.Q<Label>("LastRefreshDate");
            _refreshButton = _root.Q<Button>("RefreshButton");
            _refreshButton.clicked += RefreshData;
            RefreshData();
        }

        private void GetSizeLabels()
        {
            _texturesSizeLabel = _root.Q<Label>("TexturesSize");
            _meshesSizeLabel = _root.Q<Label>("MeshesSize");
            _animationsSizeLabel= _root.Q<Label>("AnimationsSize"); 
            _soundsSizeLabel = _root.Q<Label>("SoundsSize"); 
            _shadersSizeLabel = _root.Q<Label>("ShadersSize"); 
            _othersAssetsSizeLabel = _root.Q<Label>("OtherAssetsSize"); 
            _levelsSizeLabel = _root.Q<Label>("LevelsSize"); 
            _scriptsSizeLabel = _root.Q<Label>("ScriptsSize"); 
            _includedDLLsSizeLabel = _root.Q<Label>("IncludedDLLsSize"); 
            _fileHeadersSizeLabel = _root.Q<Label>("FileHeadersSize"); 
            _totalUserAssetsSizeLabel = _root.Q<Label>("TotalUserAssetsSize"); 
            _completeBuildSizeLabel = _root.Q<Label>("CompleteBuildSize");
        }

        private void GetPercentageLabels()
        {
            _texturesPercentageLabel = _root.Q<Label>("TexturesPercentage");
            _meshesPercentageLabel = _root.Q<Label>("MeshesPercentage");
            _animationsPercentageLabel= _root.Q<Label>("AnimationsPercentage"); 
            _soundsPercentageLabel = _root.Q<Label>("SoundsPercentage"); 
            _shadersPercentageLabel = _root.Q<Label>("ShadersPercentage"); 
            _othersAssetsPercentageLabel = _root.Q<Label>("OtherAssetsPercentage"); 
            _levelsPercentageLabel = _root.Q<Label>("LevelsPercentage"); 
            _scriptsPercentageLabel = _root.Q<Label>("ScriptsPercentage"); 
            _includedDLLsPercentageLabel = _root.Q<Label>("DLLsPercentage"); 
            _fileHeadersPercentageLabel = _root.Q<Label>("FileHeadersPercentage"); 
            _totalUserAssetsPercentageLabel = _root.Q<Label>("TotalUserAssetsPercentage");
        }
        
        /// <summary>
        /// Update UI with last build report results.
        /// </summary>
        private void RefreshData()
        {
            Dictionary<string, BuildReportEntry> reportEntries = GetLastBuildReportData(_logPath);
            if (reportEntries.Count == 0)
            {
                _lastRefreshLabel.text = "NO BUILD REPORT WAS FOUND !";
            }
            else
            {
                _lastRefreshLabel.text = $"{DateTime.Now.ToLocalTime()}";
                _texturesSizeLabel.text = $"{reportEntries[TexturesTag].Size} {reportEntries[TexturesTag].SizeUnit}";
                _meshesSizeLabel.text = $"{reportEntries[MeshesTag].Size} {reportEntries[MeshesTag].SizeUnit}";
                _animationsSizeLabel.text = $"{reportEntries[AnimationsTag].Size} {reportEntries[AnimationsTag].SizeUnit}";
                _soundsSizeLabel.text = $"{reportEntries[SoundsTag].Size} {reportEntries[SoundsTag].SizeUnit}";
                _shadersSizeLabel.text = $"{reportEntries[ShadersTag].Size} {reportEntries[ShadersTag].SizeUnit}";
                _othersAssetsSizeLabel.text = $"{reportEntries[OtherAssetsTag].Size} {reportEntries[OtherAssetsTag].SizeUnit}"; 
                _levelsSizeLabel.text = $"{reportEntries[LevelsTag].Size} {reportEntries[LevelsTag].SizeUnit}"; 
                _scriptsSizeLabel.text = $"{reportEntries[ScriptsTag].Size} {reportEntries[ScriptsTag].SizeUnit}";
                _includedDLLsSizeLabel.text = $"{reportEntries[IncludedDLLsTag].Size} {reportEntries[IncludedDLLsTag].SizeUnit}";
                _fileHeadersSizeLabel.text = $"{reportEntries[FileHeadersTag].Size} {reportEntries[FileHeadersTag].SizeUnit}";
                _totalUserAssetsSizeLabel.text = $"{reportEntries[TotalUserAssetsTag].Size} {reportEntries[TotalUserAssetsTag].SizeUnit}";
                _completeBuildSizeLabel.text = $"{reportEntries[CompleteBuildSizeTag].Size} {reportEntries[CompleteBuildSizeTag].SizeUnit}";
                _texturesPercentageLabel.text = $"{reportEntries[TexturesTag].Percentage}";
                _meshesPercentageLabel.text = $"{reportEntries[MeshesTag].Percentage}";
                _animationsPercentageLabel.text = $"{reportEntries[AnimationsTag].Percentage}";
                _soundsPercentageLabel.text = $"{reportEntries[SoundsTag].Percentage}";
                _shadersPercentageLabel.text = $"{reportEntries[ShadersTag].Percentage}"; 
                _othersAssetsPercentageLabel.text = $"{reportEntries[OtherAssetsTag].Percentage}"; 
                _levelsPercentageLabel.text = $"{reportEntries[LevelsTag].Percentage}";
                _scriptsPercentageLabel.text = $"{reportEntries[ScriptsTag].Percentage}";
                _includedDLLsPercentageLabel.text = $"{reportEntries[IncludedDLLsTag].Percentage}"; 
                _fileHeadersPercentageLabel.text = $"{reportEntries[FileHeadersTag].Percentage}"; 
                _totalUserAssetsPercentageLabel.text = $"{reportEntries[TotalUserAssetsTag].Percentage}";
            }
        }

        /// <summary>
        /// Parse last build report from given log path.
        /// </summary>
        /// <param name="logPath">File path name of log.</param>
        /// <returns>Parsed lines of build report, returned as a dictionary with item as keys an its corresponding <see cref="BuildReportEntry"/> as value.</returns>
        public static Dictionary<string, BuildReportEntry> GetLastBuildReportData(string logPath)
        {
            Dictionary<string, BuildReportEntry> entries = new Dictionary<string, BuildReportEntry>();
            string[] lines = ReadLines(logPath);
            int index = 0;
            foreach (string line in lines)
            {
                if (line.StartsWith(BuildReportTag) && lines[index + 1].StartsWith(UncompressedTag))
                {
                    entries[TexturesTag] = ParseLine(lines[index + 2]);
                    entries[MeshesTag] = ParseLine(lines[index + 3]);
                    entries[AnimationsTag] = ParseLine(lines[index + 4]);
                    entries[SoundsTag] = ParseLine(lines[index + 5]);
                    entries[ShadersTag] = ParseLine(lines[index + 6]);
                    entries[OtherAssetsTag] = ParseLine(lines[index + 7]);
                    entries[LevelsTag] = ParseLine(lines[index + 8]);
                    entries[ScriptsTag] = ParseLine(lines[index + 9]);
                    entries[IncludedDLLsTag] = ParseLine(lines[index + 10]);
                    entries[FileHeadersTag] = ParseLine(lines[index + 11]);
                    entries[TotalUserAssetsTag] = ParseLine(lines[index + 12]);
                    entries[CompleteBuildSizeTag] = ParseLine(lines[index + 13]);
                }
                index++;
            }
            return entries;
        }

        /// <summary>
        /// Custom all-lines file reader.
        ///
        /// I cannot use built-in File.ReadAllLines() because Editor.log is already open by unity and
        /// build ReadAllLines gives me an IOException about it cannot share access. So, I had to make
        /// my own reader setting opening parameters to share access to file with Unity process.
        /// </summary>
        /// <param name="logPath">Log file path name.</param>
        /// <returns>An array with every read line.</returns>
        private static string[] ReadLines(string logPath)
        {
            List<String> lines = new List<String>();
            using (FileStream fileStream = new FileStream(logPath,FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                String line;
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                {
                    while ((line = streamReader.ReadLine()) != null)
                        lines.Add(line);
                }
            }
            return lines.ToArray();
        }

        /// <summary>
        /// Take tokens from given build report line and return a BuildReportEntry.
        /// </summary>
        /// <param name="line">Build report line</param>
        /// <returns>Created BuildReportEntry</returns>
        public static BuildReportEntry ParseLine(string line)
        {
            Match match = rx.Match(line);
            BuildReportEntry entry = new BuildReportEntry(
                size: match.Groups[1].Value,
                sizeUnit: match.Groups[2].Value.ToUpper(),
                percentage: match.Groups[3].Value);
            return entry;
        }
        
    }
}