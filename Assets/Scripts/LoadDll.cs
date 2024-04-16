using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoadDll : MonoBehaviour
{
	public GameObject test;

	void Start()
	{
		// Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
#if !UNITY_EDITOR
        Assembly hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));
#else
		// Editor下无需加载，直接查找获得HotUpdate程序集
		Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
		Type type = hotUpdateAss.GetType("test");
		type.GetMethod("Run").Invoke(null, null);

	}

}



// using HybridCLR;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using System.Threading.Tasks;
// using UnityEngine;
// using UnityEngine.Networking;
// using YooAsset;

// public class LoadDll : MonoBehaviour
// {
//     public EPlayMode playMode = EPlayMode.EditorSimulateMode;

//     void Start()
//     {
//         // StartCoroutine(DownLoadAssets(this.StartGame));
//         StartCoroutine(DownLoadAssetsByYooAsset(this.StartGame));
//     }

//     #region download assets

//     private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

//     public static byte[] ReadBytesFromStreamingAssets(string dllName)
//     {
//         return s_assetDatas[dllName];
//     }

//     private string GetWebRequestPath(string asset)
//     {
//         var path = $"{Application.streamingAssetsPath}/{asset}";
//         if (!path.Contains("://"))
//         {
//             path = "file://" + path;
//         }
//         return path;
//     }
//     private static List<string> AOTMetaAssemblyFiles { get; } = new List<string>()
//     {
//         "mscorlib.dll.bytes",
//         "System.dll.bytes",
//         "System.Core.dll.bytes",
//     };



//     IEnumerator DownLoadAssetsByYooAsset(Action onDownloadComplete)
//     {
//         yield return null;
//         // 初始化资源系统
//         YooAssets.Initialize();

//         // 创建默认的资源包
//         var package = YooAssets.CreatePackage("DefaultPackage");

//         // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
//         YooAssets.SetDefaultPackage(package);

//         if (playMode == EPlayMode.EditorSimulateMode)
//         {
//             // 编辑器运行模式
//             var initParameters = new EditorSimulateModeParameters();
//             var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
//             initParameters.SimulateManifestFilePath = simulateManifestFilePath;
//             yield return package.InitializeAsync(initParameters);
//         }
//         else if (playMode == EPlayMode.OfflinePlayMode)
//         {
//             // 单机运行模式
//             var initParameters = new OfflinePlayModeParameters();
//             yield return package.InitializeAsync(initParameters);
//         }
//         else if (playMode == EPlayMode.HostPlayMode)
//         {
//             // 联机运行模式
//             // 注意：GameQueryServices.cs 太空战机的脚本类，详细见StreamingAssetsHelper.cs
//             string defaultHostServer = "http://127.0.0.1:8080/TakeTogether/package/";
//             string fallbackHostServer = "http://127.0.0.1:8080/TakeTogether/package/";
//             var initParameters = new HostPlayModeParameters();
//             initParameters.BuildinQueryServices = new GameQueryServices();
//             initParameters.DecryptionServices = new FileOffsetDecryption();
//             initParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
//             var initOperation = package.InitializeAsync(initParameters);
//             yield return initOperation;

//             if (initOperation.Status == EOperationStatus.Succeed)
//             {
//                 Debug.Log("资源包初始化成功！");
//             }
//             else
//             {
//                 Debug.LogError($"资源包初始化失败：{initOperation.Error}");
//             }
//         }

//         var operation = package.UpdatePackageVersionAsync();
//         yield return operation;

//         string packageVersion;
//         if (operation.Status == EOperationStatus.Succeed)
//         {
//             //更新成功
//             packageVersion = operation.PackageVersion;
//             Debug.Log($"Updated package Version : {packageVersion}");
//         }
//         else
//         {
//             //更新失败
//             Debug.LogError(operation.Error);
//             yield break;
//         }


//         // 更新成功后自动保存版本号，作为下次初始化的版本。
//         // 也可以通过operation.SavePackageVersion()方法保存。
//         bool savePackageVersion = true;
//         var operation2 = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
//         yield return operation2;

//         if (operation2.Status == EOperationStatus.Succeed)
//         {
//             //更新成功
//         }
//         else
//         {
//             //更新失败
//             Debug.LogError(operation2.Error);
//         }

//         yield return Download();

//         var assets = new List<string>
//         {
//             "HotUpdate.dll.bytes",
//         }.Concat(AOTMetaAssemblyFiles);

//         foreach (var asset in assets)
//         {
//             RawFileHandle handle = package.LoadRawFileAsync(asset);
//             yield return handle;
//             byte[] fileData = handle.GetRawFileData();
//             s_assetDatas[asset] = fileData;
//             print("dll:" + asset + "  size:" + fileData.Length);
//             // string fileText = handle.GetRawFileText();
//             // string filePath = handle.GetRawFilePath();
//         }


//     }

//     IEnumerator Download()
//     {
//         int downloadingMaxNum = 10;
//         int failedTryAgain = 3;
//         var package = YooAssets.GetPackage("DefaultPackage");
//         var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

//         //没有需要下载的资源
//         if (downloader.TotalDownloadCount == 0)
//         {
//             yield break;
//         }

//         //需要下载的文件总数和总大小
//         int totalDownloadCount = downloader.TotalDownloadCount;
//         long totalDownloadBytes = downloader.TotalDownloadBytes;

//         //注册回调方法
//         downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
//         downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
//         downloader.OnDownloadOverCallback = OnDownloadOverFunction;
//         downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;

//         //开启下载
//         downloader.BeginDownload();
//         yield return downloader;

//         //检测下载结果
//         if (downloader.Status == EOperationStatus.Succeed)
//         {
//             //下载成功
//             print("更新完成");
//         }
//         else
//         {
//             //下载失败
//             print("更新失败");
//         }
//     }

//     private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
//     {
//         print("开始下载：文件名：" + fileName + "文件大小" + sizeBytes);
//     }

//     private void OnDownloadOverFunction(bool isSucceed)
//     {
//         print("下载" + (isSucceed ? "成功" : "失败"));
//     }

//     private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
//     {
//         print("文件总数：" + totalDownloadCount + "已下载文件数：" +
//             currentDownloadCount + "总下载大小：" + totalDownloadBytes +
//                 "已下载大小：" + currentDownloadBytes);
//     }

//     private void OnDownloadErrorFunction(string fileName, string error)
//     {
//         print("下载失败：文件名：" + fileName + "错误信息：" + error);
//     }



//     IEnumerator DownLoadAssets(Action onDownloadComplete)
//     {
//         var assets = new List<string>
//                 {
//                         "HotUpdate.dll.bytes",
//                 }.Concat(AOTMetaAssemblyFiles);

//         foreach (var asset in assets)
//         {
//             string dllPath = GetWebRequestPath(asset);
//             Debug.Log($"start download asset:{dllPath}");
//             UnityWebRequest www = UnityWebRequest.Get(dllPath);
//             yield return www.SendWebRequest();

// #if UNITY_2020_1_OR_NEWER
//             if (www.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.Log(www.error);
//             }
// #else
//             if (www.isHttpError || www.isNetworkError)
//             {
//                 Debug.Log(www.error);
//             }
// #endif
//             else
//             {
//                 // Or retrieve results as binary data
//                 byte[] assetData = www.downloadHandler.data;
//                 Debug.Log($"dll:{asset}  size:{assetData.Length}");
//                 s_assetDatas[asset] = assetData;
//             }
//         }

//         onDownloadComplete();
//     }

//     #endregion

//     private static Assembly _hotUpdateAss;

//     /// <summary>
//     /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
//     /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
//     /// </summary>
//     private static void LoadMetadataForAOTAssemblies()
//     {
//         /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
//         /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
//         /// 
//         HomologousImageMode mode = HomologousImageMode.SuperSet;
//         foreach (var aotDllName in AOTMetaAssemblyFiles)
//         {
//             byte[] dllBytes = ReadBytesFromStreamingAssets(aotDllName);
//             // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
//             LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
//             Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
//         }
//     }

//     void StartGame()
//     {
//         LoadMetadataForAOTAssemblies();
// #if !UNITY_EDITOR
//         _hotUpdateAss = Assembly.Load(ReadBytesFromStreamingAssets("HotUpdate.dll.bytes"));
// #else
//         _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
// #endif
//         Type entryType = _hotUpdateAss.GetType("Entry");
//         entryType.GetMethod("Start").Invoke(null, null);

//         //Run_InstantiateComponentByAsset();

//         StartCoroutine(DelayAndQuit());
//     }

//     IEnumerator DelayAndQuit()
//     {
// #if UNITY_STANDALONE_WIN
//         File.WriteAllText(Directory.GetCurrentDirectory() + "/run.log", "ok", System.Text.Encoding.UTF8);
// #endif
//         for (int i = 10; i >= 1; i--)
//         {
//             UnityEngine.Debug.Log($"将于{i}s后自动退出");
//             yield return new WaitForSeconds(1f);
//         }
//         Application.Quit();
//     }

//     private static void Run_InstantiateComponentByAsset()
//     {
//         // 通过实例化assetbundle中的资源，还原资源上的热更新脚本
//         AssetBundle ab = AssetBundle.LoadFromMemory(LoadDll.ReadBytesFromStreamingAssets("prefabs"));
//         GameObject cube = ab.LoadAsset<GameObject>("Cube");
//         GameObject.Instantiate(cube);
//     }
// }


// /// <summary>
// /// 资源文件偏移加载解密类
// /// </summary>
// public class FileOffsetDecryption : IDecryptionServices
// {
//     /// <summary>
//     /// 同步方式获取解密的资源包对象
//     /// 注意：加载流对象在资源包对象释放的时候会自动释放
//     /// </summary>
//     AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
//     {
//         managedStream = null;
//         return AssetBundle.LoadFromFile(fileInfo.FileLoadPath, fileInfo.ConentCRC, GetFileOffset());
//     }

//     /// <summary>
//     /// 异步方式获取解密的资源包对象
//     /// 注意：加载流对象在资源包对象释放的时候会自动释放
//     /// </summary>
//     AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
//     {
//         managedStream = null;
//         return AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, fileInfo.ConentCRC, GetFileOffset());
//     }

//     private static ulong GetFileOffset()
//     {
//         return 32;
//     }
// }

// /// <summary>
// /// 远端资源地址查询服务类
// /// </summary>
// class RemoteServices : IRemoteServices
// {
//     private readonly string _defaultHostServer;
//     private readonly string _fallbackHostServer;

//     public RemoteServices(string defaultHostServer, string fallbackHostServer)
//     {
//         _defaultHostServer = defaultHostServer;
//         _fallbackHostServer = fallbackHostServer;
//     }
//     string IRemoteServices.GetRemoteMainURL(string fileName)
//     {
//         return $"{_defaultHostServer}/{fileName}";
//     }
//     string IRemoteServices.GetRemoteFallbackURL(string fileName)
//     {
//         return $"{_fallbackHostServer}/{fileName}";
//     }
// }