using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Widget
{
    public static class AddressableHelper
    {
        private static readonly Dictionary<object, AsyncOperationHandle<GameObject>> Cache =
            new Dictionary<object, AsyncOperationHandle<GameObject>>();

        public static void LoadAsync(AssetReference assetReference, Action<GameObject> onCompleteAction)
        {
            if (!assetReference.RuntimeKeyIsValid())
            {
                Debug.LogError("AssetReference has no runtime key defined!");
            }

            if (Cache.ContainsKey(assetReference.RuntimeKey))
            {
                Cache.TryGetValue(assetReference.RuntimeKey, out AsyncOperationHandle<GameObject> asyncOperationHandle);

                if (asyncOperationHandle.IsDone)
                {
                    onCompleteAction(asyncOperationHandle.Result);
                }
                else
                {
                    SetAsyncOperationHandle(asyncOperationHandle, handle => onCompleteAction(handle.Result));
                }
            }
            else
            {
                AsyncOperationHandle<GameObject> asyncOperationHandle =
                    LoadAssetAsyncAndGetOperation<GameObject>(assetReference,
                        handle => onCompleteAction(handle.Result));
                Cache[assetReference.RuntimeKey] = asyncOperationHandle;
            }
        }

        private static AsyncOperationHandle<T> LoadAssetAsyncAndGetOperation<T>(AssetReference assetReference,
            Action<AsyncOperationHandle<T>> succeededAction) where T : class
        {
            AsyncOperationHandle<T> operation = Addressables.LoadAssetAsync<T>(assetReference);
            SetAsyncOperationHandle(operation, succeededAction);
            return operation;
        }

        private static void SetAsyncOperationHandle<T>(AsyncOperationHandle<T> operation,
            Action<AsyncOperationHandle<T>> succeededAction) where T : class
        {
            operation.Completed += handle =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.None:
                        break;
                    case AsyncOperationStatus.Succeeded:
                        succeededAction?.Invoke(handle);
                        break;
                    case AsyncOperationStatus.Failed:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }
    }
}