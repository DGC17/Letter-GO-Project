/*==============================================================================
Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;

namespace Vuforia
{

    /// <summary>
    /// This MonoBehaviour manages the usage of a webcam for Play Mode in Windows or Mac.
    /// </summary>
	public class WebCamBehaviour : WebCamAbstractBehaviour//, ITrackerEventHandler
    {
		/*
		private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGB888;
		private bool m_RegisteredFormat = false;
		private bool m_LogInfo = true;

		static bool hideButton = false;

		void Start() {
			VuforiaBehaviour vuforiaBehaviour = (VuforiaBehaviour) FindObjectOfType(typeof(VuforiaBehaviour));
			if (vuforiaBehaviour)
				vuforiaBehaviour.RegisterTrackerEventHandler (this);
		}

		public void OnInitialized () {

		}

		public void OnTrackablesUpdated()
		{
			if (!m_RegisteredFormat) {
				CameraDevice.Instance.SetFrameFormat (m_PixelFormat, true);
				m_RegisteredFormat = true;
			}

			if (m_LogInfo) {
				CameraDevice cam = CameraDevice.Instance;
				Image image = cam.GetCameraImage (m_PixelFormat);
				if (image != null) {
					m_LogInfo = false;
				}
			}
		}

		void TakeScreenShot() {
			CameraDevice cam = CameraDevice.Instance;
			Image image = cam.GetCameraImage (m_PixelFormat);
			if (image != null) {
				m_LogInfo = false;
				Texture2D tex = new Texture2D (image.Width, image.Height, TextureFormat.RGB24, false);
				image.CopyToTexture (tex);

				tex.Apply ();

				byte[] bytes = tex.EncodeToPNG ();
				Destroy (tex);

				//File.WriteAllBytes (Application.persistentDataPath + "/Screen.png", bytes);
			}
		}

		void OnGUI () {
			if (!hideButton) {
				if (GUI.Button (new Rect (40, 80, 160, 40), "Take Picture")) {
					hideButton = true;
					TakeScreenShot ();
				}
			}
		}
		*/
    }
}
