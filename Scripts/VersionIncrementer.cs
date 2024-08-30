using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace NeKoRoSYS.VersionControl
{
    /** Based on LlamAcademy's tutrial https://www.youtube.com/watch?v=PbFE0m9UMtE. */
    public class Build : ScriptableObject
    {
        public string buildNumber = "1";
        public string bundleVersion;
    }

    public class VersionIncrementer : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        public void OnPreprocessBuild(BuildReport report) => UpdateBuildVersion(report);
        private void UpdateBuildVersion(BuildReport report) // MM = Month; DD = Day; YY or YYYY = Year
        {
            Build buildObject = ScriptableObject.CreateInstance<Build>();
            switch(report.summary.platform)
            {
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneOSX:
                    PlayerSettings.macOS.buildNumber = IncrementBuildNumber(PlayerSettings.macOS.buildNumber);
                    buildObject.buildNumber = PlayerSettings.macOS.buildNumber;
                break;
                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = IncrementBuildNumber(PlayerSettings.iOS.buildNumber);
                    buildObject.buildNumber = PlayerSettings.iOS.buildNumber;
                break;
                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode++;
                    buildObject.buildNumber = PlayerSettings.Android.bundleVersionCode.ToString();
                break;
                case BuildTarget.PS4:
                    PlayerSettings.PS4.appVersion = IncrementBuildNumber(PlayerSettings.PS4.appVersion);
                    buildObject.buildNumber = PlayerSettings.PS4.appVersion;
                break;
                case BuildTarget.XboxOne:
                    PlayerSettings.XboxOne.Version = IncrementBuildNumber(PlayerSettings.XboxOne.Version);
                    buildObject.buildNumber = PlayerSettings.XboxOne.Version;
                break;
                case BuildTarget.WSAPlayer:
                    PlayerSettings.WSA.packageVersion = new(PlayerSettings.WSA.packageVersion.Major, PlayerSettings.WSA.packageVersion.Minor, PlayerSettings.WSA.packageVersion.Build + 1);
                    buildObject.buildNumber = PlayerSettings.WSA.packageVersion.Build.ToString();
                break;
                case BuildTarget.Switch:
                    PlayerSettings.Switch.displayVersion = IncrementBuildNumber(PlayerSettings.Switch.displayVersion);
                    PlayerSettings.Switch.releaseVersion = IncrementBuildNumber(PlayerSettings.Switch.releaseVersion);
                    buildObject.buildNumber = PlayerSettings.Switch.displayVersion;
                break;
                case BuildTarget.tvOS:
                    PlayerSettings.tvOS.buildNumber = IncrementBuildNumber(PlayerSettings.tvOS.buildNumber);
                    buildObject.buildNumber = PlayerSettings.tvOS.buildNumber;
                break;
            }
            PlayerSettings.bundleVersion = $"{BaseVersionEditor.GetBaseVersion()}.{DateTime.Now:mmddyy}.{buildObject.buildNumber}";
            buildObject.bundleVersion = PlayerSettings.bundleVersion;
            AssetDatabase.DeleteAsset("Assets/Runtime/Resources/System/Build.asset");
            AssetDatabase.CreateAsset(buildObject, "Assets/Runtime/Resources/System/Build.asset");
            AssetDatabase.SaveAssets();
            Debug.Log($"Updated version to {PlayerSettings.bundleVersion}!");
        }

        private string IncrementBuildNumber(string buildNumber)
        {
            int.TryParse(buildNumber, out int outputBuildNumber);
            return (outputBuildNumber + 1).ToString();
        }
    }
}
