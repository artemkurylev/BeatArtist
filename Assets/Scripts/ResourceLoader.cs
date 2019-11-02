using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;
using System;

namespace Resources
{
    
    
    public class ResourceLoader
    {
        public ResourceLoader() { }

        public IEnumerator GetMenuTrackBackground(int trackId, RawImage response)
        {
            string progress = GetTrackBackgroundProgress(trackId);
            string path = Application.persistentDataPath;
            // string path = Application.dataPath + "/Resources/track-" + trackId + "-" + progress + ".png
            if (trackId == 1)
            {
                path = Application.persistentDataPath + "/Resources/art_43/layer2.png";
            }

            string url = "file://" + path;
            var request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            Debug.Log(url);
            if (!request.isHttpError && !request.isNetworkError)
            {
                Debug.Log(DownloadHandlerTexture.GetContent(request));
                response.texture = DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                response.texture = null;
            }

            request.Dispose();
        }

        private string GetTrackBackgroundProgress(int trackId)
        {
            return "0";
        }
    }
}
