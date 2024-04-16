using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Mirror.Components.dll",
		"Mirror.dll",
		"UnityEngine.CoreModule.dll",
		"YooAsset.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Mirror.Discovery.NetworkDiscoveryBase<Mirror.Discovery.ServerRequest,Mirror.Discovery.ServerResponse>
	// Mirror.Reader<Mirror.AddPlayerMessage>
	// Mirror.Reader<Mirror.ChangeOwnerMessage>
	// Mirror.Reader<Mirror.CommandMessage>
	// Mirror.Reader<Mirror.EntityStateMessage>
	// Mirror.Reader<Mirror.NetworkBehaviourSyncVar>
	// Mirror.Reader<Mirror.NetworkPingMessage>
	// Mirror.Reader<Mirror.NetworkPongMessage>
	// Mirror.Reader<Mirror.NotReadyMessage>
	// Mirror.Reader<Mirror.ObjectDestroyMessage>
	// Mirror.Reader<Mirror.ObjectHideMessage>
	// Mirror.Reader<Mirror.ObjectSpawnFinishedMessage>
	// Mirror.Reader<Mirror.ObjectSpawnStartedMessage>
	// Mirror.Reader<Mirror.ReadyMessage>
	// Mirror.Reader<Mirror.RpcMessage>
	// Mirror.Reader<Mirror.SceneMessage>
	// Mirror.Reader<Mirror.SpawnMessage>
	// Mirror.Reader<Mirror.TimeSnapshotMessage>
	// Mirror.Reader<System.ArraySegment<byte>>
	// Mirror.Reader<System.DateTime>
	// Mirror.Reader<System.Decimal>
	// Mirror.Reader<System.Guid>
	// Mirror.Reader<System.Nullable<System.DateTime>>
	// Mirror.Reader<System.Nullable<System.Decimal>>
	// Mirror.Reader<System.Nullable<System.Guid>>
	// Mirror.Reader<System.Nullable<UnityEngine.Color32>>
	// Mirror.Reader<System.Nullable<UnityEngine.Color>>
	// Mirror.Reader<System.Nullable<UnityEngine.LayerMask>>
	// Mirror.Reader<System.Nullable<UnityEngine.Matrix4x4>>
	// Mirror.Reader<System.Nullable<UnityEngine.Plane>>
	// Mirror.Reader<System.Nullable<UnityEngine.Quaternion>>
	// Mirror.Reader<System.Nullable<UnityEngine.Ray>>
	// Mirror.Reader<System.Nullable<UnityEngine.Rect>>
	// Mirror.Reader<System.Nullable<UnityEngine.Vector2>>
	// Mirror.Reader<System.Nullable<UnityEngine.Vector2Int>>
	// Mirror.Reader<System.Nullable<UnityEngine.Vector3>>
	// Mirror.Reader<System.Nullable<UnityEngine.Vector3Int>>
	// Mirror.Reader<System.Nullable<UnityEngine.Vector4>>
	// Mirror.Reader<System.Nullable<byte>>
	// Mirror.Reader<System.Nullable<double>>
	// Mirror.Reader<System.Nullable<float>>
	// Mirror.Reader<System.Nullable<int>>
	// Mirror.Reader<System.Nullable<long>>
	// Mirror.Reader<System.Nullable<sbyte>>
	// Mirror.Reader<System.Nullable<short>>
	// Mirror.Reader<System.Nullable<uint>>
	// Mirror.Reader<System.Nullable<ulong>>
	// Mirror.Reader<System.Nullable<ushort>>
	// Mirror.Reader<UnityEngine.Color32>
	// Mirror.Reader<UnityEngine.Color>
	// Mirror.Reader<UnityEngine.LayerMask>
	// Mirror.Reader<UnityEngine.Matrix4x4>
	// Mirror.Reader<UnityEngine.Plane>
	// Mirror.Reader<UnityEngine.Quaternion>
	// Mirror.Reader<UnityEngine.Ray>
	// Mirror.Reader<UnityEngine.Rect>
	// Mirror.Reader<UnityEngine.Vector2>
	// Mirror.Reader<UnityEngine.Vector2Int>
	// Mirror.Reader<UnityEngine.Vector3>
	// Mirror.Reader<UnityEngine.Vector3Int>
	// Mirror.Reader<UnityEngine.Vector4>
	// Mirror.Reader<byte>
	// Mirror.Reader<double>
	// Mirror.Reader<float>
	// Mirror.Reader<int>
	// Mirror.Reader<long>
	// Mirror.Reader<object>
	// Mirror.Reader<sbyte>
	// Mirror.Reader<short>
	// Mirror.Reader<uint>
	// Mirror.Reader<ulong>
	// Mirror.Reader<ushort>
	// Mirror.Writer<Mirror.AddPlayerMessage>
	// Mirror.Writer<Mirror.ChangeOwnerMessage>
	// Mirror.Writer<Mirror.CommandMessage>
	// Mirror.Writer<Mirror.EntityStateMessage>
	// Mirror.Writer<Mirror.NetworkPingMessage>
	// Mirror.Writer<Mirror.NetworkPongMessage>
	// Mirror.Writer<Mirror.NotReadyMessage>
	// Mirror.Writer<Mirror.ObjectDestroyMessage>
	// Mirror.Writer<Mirror.ObjectHideMessage>
	// Mirror.Writer<Mirror.ObjectSpawnFinishedMessage>
	// Mirror.Writer<Mirror.ObjectSpawnStartedMessage>
	// Mirror.Writer<Mirror.ReadyMessage>
	// Mirror.Writer<Mirror.RpcMessage>
	// Mirror.Writer<Mirror.SceneMessage>
	// Mirror.Writer<Mirror.SpawnMessage>
	// Mirror.Writer<Mirror.TimeSnapshotMessage>
	// Mirror.Writer<System.ArraySegment<byte>>
	// Mirror.Writer<System.DateTime>
	// Mirror.Writer<System.Decimal>
	// Mirror.Writer<System.Guid>
	// Mirror.Writer<System.Nullable<System.DateTime>>
	// Mirror.Writer<System.Nullable<System.Decimal>>
	// Mirror.Writer<System.Nullable<System.Guid>>
	// Mirror.Writer<System.Nullable<UnityEngine.Color32>>
	// Mirror.Writer<System.Nullable<UnityEngine.Color>>
	// Mirror.Writer<System.Nullable<UnityEngine.LayerMask>>
	// Mirror.Writer<System.Nullable<UnityEngine.Matrix4x4>>
	// Mirror.Writer<System.Nullable<UnityEngine.Plane>>
	// Mirror.Writer<System.Nullable<UnityEngine.Quaternion>>
	// Mirror.Writer<System.Nullable<UnityEngine.Ray>>
	// Mirror.Writer<System.Nullable<UnityEngine.Rect>>
	// Mirror.Writer<System.Nullable<UnityEngine.Vector2>>
	// Mirror.Writer<System.Nullable<UnityEngine.Vector2Int>>
	// Mirror.Writer<System.Nullable<UnityEngine.Vector3>>
	// Mirror.Writer<System.Nullable<UnityEngine.Vector3Int>>
	// Mirror.Writer<System.Nullable<UnityEngine.Vector4>>
	// Mirror.Writer<System.Nullable<byte>>
	// Mirror.Writer<System.Nullable<double>>
	// Mirror.Writer<System.Nullable<float>>
	// Mirror.Writer<System.Nullable<int>>
	// Mirror.Writer<System.Nullable<long>>
	// Mirror.Writer<System.Nullable<sbyte>>
	// Mirror.Writer<System.Nullable<short>>
	// Mirror.Writer<System.Nullable<uint>>
	// Mirror.Writer<System.Nullable<ulong>>
	// Mirror.Writer<System.Nullable<ushort>>
	// Mirror.Writer<UnityEngine.Color32>
	// Mirror.Writer<UnityEngine.Color>
	// Mirror.Writer<UnityEngine.LayerMask>
	// Mirror.Writer<UnityEngine.Matrix4x4>
	// Mirror.Writer<UnityEngine.Plane>
	// Mirror.Writer<UnityEngine.Quaternion>
	// Mirror.Writer<UnityEngine.Ray>
	// Mirror.Writer<UnityEngine.Rect>
	// Mirror.Writer<UnityEngine.Vector2>
	// Mirror.Writer<UnityEngine.Vector2Int>
	// Mirror.Writer<UnityEngine.Vector3>
	// Mirror.Writer<UnityEngine.Vector3Int>
	// Mirror.Writer<UnityEngine.Vector4>
	// Mirror.Writer<byte>
	// Mirror.Writer<double>
	// Mirror.Writer<float>
	// Mirror.Writer<int>
	// Mirror.Writer<long>
	// Mirror.Writer<object>
	// Mirror.Writer<sbyte>
	// Mirror.Writer<short>
	// Mirror.Writer<uint>
	// Mirror.Writer<ulong>
	// Mirror.Writer<ushort>
	// System.Action<UnityEngine.ParticleSystem.Particle>
	// System.Action<UnityEngine.Vector2>
	// System.Action<UnityEngine.Vector2Int>
	// System.Action<UnityEngine.Vector3>
	// System.Action<UnityEngine.Vector3Int>
	// System.Action<byte,byte>
	// System.Action<double,double>
	// System.Action<float,float>
	// System.Action<float>
	// System.Action<int,int>
	// System.Action<object,Mirror.AddPlayerMessage>
	// System.Action<object,Mirror.ChangeOwnerMessage>
	// System.Action<object,Mirror.CommandMessage>
	// System.Action<object,Mirror.EntityStateMessage>
	// System.Action<object,Mirror.NetworkPingMessage>
	// System.Action<object,Mirror.NetworkPongMessage>
	// System.Action<object,Mirror.NotReadyMessage>
	// System.Action<object,Mirror.ObjectDestroyMessage>
	// System.Action<object,Mirror.ObjectHideMessage>
	// System.Action<object,Mirror.ObjectSpawnFinishedMessage>
	// System.Action<object,Mirror.ObjectSpawnStartedMessage>
	// System.Action<object,Mirror.ReadyMessage>
	// System.Action<object,Mirror.RpcMessage>
	// System.Action<object,Mirror.SceneMessage>
	// System.Action<object,Mirror.SpawnMessage>
	// System.Action<object,Mirror.TimeSnapshotMessage>
	// System.Action<object,System.ArraySegment<byte>>
	// System.Action<object,System.DateTime>
	// System.Action<object,System.Decimal>
	// System.Action<object,System.Guid>
	// System.Action<object,System.Nullable<System.DateTime>>
	// System.Action<object,System.Nullable<System.Decimal>>
	// System.Action<object,System.Nullable<System.Guid>>
	// System.Action<object,System.Nullable<UnityEngine.Color32>>
	// System.Action<object,System.Nullable<UnityEngine.Color>>
	// System.Action<object,System.Nullable<UnityEngine.LayerMask>>
	// System.Action<object,System.Nullable<UnityEngine.Matrix4x4>>
	// System.Action<object,System.Nullable<UnityEngine.Plane>>
	// System.Action<object,System.Nullable<UnityEngine.Quaternion>>
	// System.Action<object,System.Nullable<UnityEngine.Ray>>
	// System.Action<object,System.Nullable<UnityEngine.Rect>>
	// System.Action<object,System.Nullable<UnityEngine.Vector2>>
	// System.Action<object,System.Nullable<UnityEngine.Vector2Int>>
	// System.Action<object,System.Nullable<UnityEngine.Vector3>>
	// System.Action<object,System.Nullable<UnityEngine.Vector3Int>>
	// System.Action<object,System.Nullable<UnityEngine.Vector4>>
	// System.Action<object,System.Nullable<byte>>
	// System.Action<object,System.Nullable<double>>
	// System.Action<object,System.Nullable<float>>
	// System.Action<object,System.Nullable<int>>
	// System.Action<object,System.Nullable<long>>
	// System.Action<object,System.Nullable<sbyte>>
	// System.Action<object,System.Nullable<short>>
	// System.Action<object,System.Nullable<uint>>
	// System.Action<object,System.Nullable<ulong>>
	// System.Action<object,System.Nullable<ushort>>
	// System.Action<object,UnityEngine.Color32>
	// System.Action<object,UnityEngine.Color>
	// System.Action<object,UnityEngine.LayerMask>
	// System.Action<object,UnityEngine.Matrix4x4>
	// System.Action<object,UnityEngine.Plane>
	// System.Action<object,UnityEngine.Quaternion>
	// System.Action<object,UnityEngine.Ray>
	// System.Action<object,UnityEngine.Rect>
	// System.Action<object,UnityEngine.Vector2>
	// System.Action<object,UnityEngine.Vector2Int>
	// System.Action<object,UnityEngine.Vector3>
	// System.Action<object,UnityEngine.Vector3Int>
	// System.Action<object,UnityEngine.Vector4>
	// System.Action<object,byte>
	// System.Action<object,double>
	// System.Action<object,float>
	// System.Action<object,int>
	// System.Action<object,long>
	// System.Action<object,object>
	// System.Action<object,sbyte>
	// System.Action<object,short>
	// System.Action<object,uint>
	// System.Action<object,ulong>
	// System.Action<object,ushort>
	// System.Action<object>
	// System.ArraySegment.Enumerator<byte>
	// System.ArraySegment<byte>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<long,Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<long,Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,byte>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<double>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,Mirror.Discovery.ServerResponse>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,Mirror.Discovery.ServerResponse>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,Mirror.Discovery.ServerResponse>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<long,Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,byte>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<UnityEngine.ParticleSystem.Particle>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<Mirror.Discovery.ServerResponse>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<double>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.ParticleSystem.Particle>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<UnityEngine.ParticleSystem.Particle>
	// System.Comparison<object>
	// System.Func<object,Mirror.AddPlayerMessage>
	// System.Func<object,Mirror.ChangeOwnerMessage>
	// System.Func<object,Mirror.CommandMessage>
	// System.Func<object,Mirror.EntityStateMessage>
	// System.Func<object,Mirror.NetworkBehaviourSyncVar>
	// System.Func<object,Mirror.NetworkPingMessage>
	// System.Func<object,Mirror.NetworkPongMessage>
	// System.Func<object,Mirror.NotReadyMessage>
	// System.Func<object,Mirror.ObjectDestroyMessage>
	// System.Func<object,Mirror.ObjectHideMessage>
	// System.Func<object,Mirror.ObjectSpawnFinishedMessage>
	// System.Func<object,Mirror.ObjectSpawnStartedMessage>
	// System.Func<object,Mirror.ReadyMessage>
	// System.Func<object,Mirror.RpcMessage>
	// System.Func<object,Mirror.SceneMessage>
	// System.Func<object,Mirror.SpawnMessage>
	// System.Func<object,Mirror.TimeSnapshotMessage>
	// System.Func<object,System.ArraySegment<byte>>
	// System.Func<object,System.DateTime>
	// System.Func<object,System.Decimal>
	// System.Func<object,System.Guid>
	// System.Func<object,System.Nullable<System.DateTime>>
	// System.Func<object,System.Nullable<System.Decimal>>
	// System.Func<object,System.Nullable<System.Guid>>
	// System.Func<object,System.Nullable<UnityEngine.Color32>>
	// System.Func<object,System.Nullable<UnityEngine.Color>>
	// System.Func<object,System.Nullable<UnityEngine.LayerMask>>
	// System.Func<object,System.Nullable<UnityEngine.Matrix4x4>>
	// System.Func<object,System.Nullable<UnityEngine.Plane>>
	// System.Func<object,System.Nullable<UnityEngine.Quaternion>>
	// System.Func<object,System.Nullable<UnityEngine.Ray>>
	// System.Func<object,System.Nullable<UnityEngine.Rect>>
	// System.Func<object,System.Nullable<UnityEngine.Vector2>>
	// System.Func<object,System.Nullable<UnityEngine.Vector2Int>>
	// System.Func<object,System.Nullable<UnityEngine.Vector3>>
	// System.Func<object,System.Nullable<UnityEngine.Vector3Int>>
	// System.Func<object,System.Nullable<UnityEngine.Vector4>>
	// System.Func<object,System.Nullable<byte>>
	// System.Func<object,System.Nullable<double>>
	// System.Func<object,System.Nullable<float>>
	// System.Func<object,System.Nullable<int>>
	// System.Func<object,System.Nullable<long>>
	// System.Func<object,System.Nullable<sbyte>>
	// System.Func<object,System.Nullable<short>>
	// System.Func<object,System.Nullable<uint>>
	// System.Func<object,System.Nullable<ulong>>
	// System.Func<object,System.Nullable<ushort>>
	// System.Func<object,UnityEngine.Color32>
	// System.Func<object,UnityEngine.Color>
	// System.Func<object,UnityEngine.LayerMask>
	// System.Func<object,UnityEngine.Matrix4x4>
	// System.Func<object,UnityEngine.Plane>
	// System.Func<object,UnityEngine.Quaternion>
	// System.Func<object,UnityEngine.Ray>
	// System.Func<object,UnityEngine.Rect>
	// System.Func<object,UnityEngine.Vector2>
	// System.Func<object,UnityEngine.Vector2Int>
	// System.Func<object,UnityEngine.Vector3>
	// System.Func<object,UnityEngine.Vector3Int>
	// System.Func<object,UnityEngine.Vector4>
	// System.Func<object,byte>
	// System.Func<object,double>
	// System.Func<object,float>
	// System.Func<object,int>
	// System.Func<object,long>
	// System.Func<object,object>
	// System.Func<object,sbyte>
	// System.Func<object,short>
	// System.Func<object,uint>
	// System.Func<object,ulong>
	// System.Func<object,ushort>
	// System.Predicate<UnityEngine.ParticleSystem.Particle>
	// System.Predicate<object>
	// }}

	public void RefMethods()
	{
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarDeserialize<byte>(byte&,System.Action<byte,byte>,byte)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarDeserialize<double>(double&,System.Action<double,double>,double)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarDeserialize<float>(float&,System.Action<float,float>,float)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarDeserialize<int>(int&,System.Action<int,int>,int)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarDeserialize<object>(object&,System.Action<object,object>,object)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarSetter<byte>(byte,byte&,ulong,System.Action<byte,byte>)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarSetter<double>(double,double&,ulong,System.Action<double,double>)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarSetter<float>(float,float&,ulong,System.Action<float,float>)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarSetter<int>(int,int&,ulong,System.Action<int,int>)
		// System.Void Mirror.NetworkBehaviour.GeneratedSyncVarSetter<object>(object,object&,ulong,System.Action<object,object>)
		// System.Void Mirror.NetworkBehaviour.SetSyncVar<byte>(byte,byte&,ulong)
		// System.Void Mirror.NetworkBehaviour.SetSyncVar<double>(double,double&,ulong)
		// System.Void Mirror.NetworkBehaviour.SetSyncVar<float>(float,float&,ulong)
		// System.Void Mirror.NetworkBehaviour.SetSyncVar<int>(int,int&,ulong)
		// System.Void Mirror.NetworkBehaviour.SetSyncVar<object>(object,object&,ulong)
		// bool Mirror.NetworkBehaviour.SyncVarEqual<byte>(byte,byte&)
		// bool Mirror.NetworkBehaviour.SyncVarEqual<double>(double,double&)
		// bool Mirror.NetworkBehaviour.SyncVarEqual<float>(float,float&)
		// bool Mirror.NetworkBehaviour.SyncVarEqual<int>(int,int&)
		// bool Mirror.NetworkBehaviour.SyncVarEqual<object>(object,object&)
		// object Mirror.NetworkReader.Read<object>()
		// System.Collections.Generic.List<object> Mirror.NetworkReaderExtensions.ReadList<object>(Mirror.NetworkReader)
		// object Mirror.NetworkReaderExtensions.ReadNetworkBehaviour<object>(Mirror.NetworkReader)
		// System.Void Mirror.NetworkWriter.Write<object>(object)
		// System.Void Mirror.NetworkWriterExtensions.WriteList<object>(Mirror.NetworkWriter,System.Collections.Generic.List<object>)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInParent<object>()
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Transform)
		// object UnityEngine.Resources.Load<object>(string)
		// YooAsset.AssetHandle YooAsset.ResourcePackage.LoadAssetAsync<object>(string,uint)
		// YooAsset.AssetHandle YooAsset.YooAssets.LoadAssetAsync<object>(string,uint)
	}
}