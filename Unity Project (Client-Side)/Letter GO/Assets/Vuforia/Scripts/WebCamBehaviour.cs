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
	public class WebCamBehaviour : WebCamAbstractBehaviour, ITrackerEventHandler
    {

		//External References.
		private CameraController cameraController;

		// Variables for the AR Camera configuration. 
		private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGB888;
		private bool m_RegisteredFormat = false;
		private bool m_LogInfo = true;

		// Functions executed on the intilization of the application. 
		void Start() {

			// Assignations. 
			cameraController = GameObject.FindGameObjectWithTag ("CameraController").GetComponent<CameraController>();

			VuforiaBehaviour vuforiaBehaviour = (VuforiaBehaviour) FindObjectOfType(typeof(VuforiaBehaviour));
			if (vuforiaBehaviour)
				vuforiaBehaviour.RegisterTrackerEventHandler (this);
		}

		// Necessary Defintion due to the Interfaces selected. 
		public void OnInitialized () {}

		//Configuration of the AR Camera. 
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

		// Captures the image shown by the AR Camera.
		// Called when the user clicks on the Button "Take Picture". 
		public void TakeScreenShot() {

			// We take an instance of the Camera and the image stored in that instance. 
			CameraDevice cam = CameraDevice.Instance;
			Image image = cam.GetCameraImage (m_PixelFormat);

			if (image != null) {
				m_LogInfo = false;

				// Defintion of different Textures representing the different steps of the picture transformation.
				Texture2D tex = new Texture2D (image.Width, image.Height, TextureFormat.RGB24, false);
				Texture2D tex_rotated = new Texture2D (image.Height, image.Width, TextureFormat.RGB24, false);
				Texture2D tex_inverted = new Texture2D (image.Height, image.Width, TextureFormat.RGB24, false);

				// Auxiliar Variables. 
				int w = image.Width;
				int h = image.Height;

				// STEP 1: Original format of the picture taked by the AR Camera. 
				image.CopyToTexture (tex);
				tex.Apply();

				// STEP 2: Rotation of the picture. 
				for (int j = 0; j < h; j++) {
					for (int i = 0; i < w; i++) {
						tex_rotated.SetPixel (j, i, tex.GetPixel(i,j));
					}
				}
				tex_rotated.Apply();

				// STEP 3: Apply mirror effect to the picture. 
				for (int j = 0; j < w; j++) {
					for (int i = 0; i < h; i++) {
						tex_inverted.SetPixel ((h - 1 - i), (w - 1 - j), tex_rotated.GetPixel(i,j));
					}
				}
				tex_inverted.Apply();

				byte[] bytes = tex_inverted.EncodeToPNG ();

				Destroy (tex);
				Destroy (tex_rotated);
				Destroy (tex_inverted);

				// We send the bit-stream to the Camera Controller. 
				cameraController.PhotoTaked (bytes);

				//File.WriteAllBytes (Application.persistentDataPath + "/screen.png", bytes);

			}
		}
    }
}