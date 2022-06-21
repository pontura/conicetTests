using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
//    [MenuItem("Assets/Asset Bundles/Build")]
//    static void Build()
//    {
//        string assetBundleDirectory = "Assets/AssetBundles/Android";
//        if (!Directory.Exists(assetBundleDirectory))
//        {
//            Directory.CreateDirectory(assetBundleDirectory);
//        }
//#if UNITY_ANDROID
//    BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None,BuildTarget.Android);
//#elif UNITY_WEBGL
//    BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None,BuildTarget.WebGL);
//#elif UNITY_STANDALONE
//        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows);
//#endif
//    }

    [MenuItem("Assets/Asset Bundles/Build Android")]
    static void BuildAndroid()
    {
        string assetBundleDirectory = "Assets/AssetBundles/Android";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.Android);
    }

    [MenuItem("Assets/Asset Bundles/Build Standalone")]
    static void BuildStandalone()
    {
        string assetBundleDirectory = "Assets/AssetBundles/Standalone";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Asset Bundles/Build WebGL")]
    static void BuildWebGL()
    {
        string assetBundleDirectory = "Assets/AssetBundles/WebGL";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.WebGL);
    }
    [MenuItem("Assets/Asset Bundles/Build iOS")]
    static void BuildiOS()
    {
        string assetBundleDirectory = "Assets/AssetBundles/iOS";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.iOS);
    }
}