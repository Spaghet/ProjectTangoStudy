using UnityEngine;
using Tango;
using System.Collections;
using System;

public class AreaLearningStartup : MonoBehaviour, ITangoLifecycle {
    private TangoApplication m_tangoApplication;

    // Use this for initialization
    void Start () {
        m_tangoApplication = FindObjectOfType<TangoApplication>();
        if(m_tangoApplication != null)
        {
            m_tangoApplication.Register(this);
            m_tangoApplication.RequestPermissions();
        }
	}

    public void OnTangoPermissions(bool permissionsGranted)
    {
        if (permissionsGranted)
        {
            var list = AreaDescription.GetList();
            AreaDescription mostRecent = null;
            AreaDescription.Metadata mostRecentMetadata = null;
            if (list == null)
            {
                Debug.LogError("Nothing in list");
                return;
            }
            if (list.Length > 0)
            {
                mostRecent = list[0];
                mostRecentMetadata = mostRecent.GetMetadata();
                foreach (var desc in list)
                {
                    var metaData = desc.GetMetadata();
                    if (metaData.m_dateTime > mostRecentMetadata.m_dateTime)
                    {
                        mostRecentMetadata = metaData;
                        mostRecent = desc;
                    }
                }
                m_tangoApplication.Startup(mostRecent);
            }
            else
            {
                Debug.LogError("No area descriptions available.");
            }
        }
    }

    public void OnTangoServiceConnected()
    {
        Debug.Log("Tango Service Connected");
    }

    public void OnTangoServiceDisconnected()
    {
        Debug.Log("Tango Service Disconnected");
    }
}
