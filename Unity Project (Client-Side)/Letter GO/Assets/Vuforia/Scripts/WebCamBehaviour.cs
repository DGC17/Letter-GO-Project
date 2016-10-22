/*==============================================================================
Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;

namespace Vuforia
{

	/// <summary>
	/// This MonoBehaviour manages the usage of a webcam for Play Mode in Windows or Mac.
	/// </summary>
	public class WebCamBehaviour : WebCamAbstractBehaviour
	{
		private UnityEngine.UI.Image selector;

		//External References.
		private CameraController cameraController;

		// Variables for the AR Camera configuration. 
		private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGB888;
		private bool m_RegisteredFormat = false;

		// Functions executed on the intilization of the application. 
		void Start() {
			// Assignations. 
			selector = GameObject.Find ("GI.Selector").GetComponent<UnityEngine.UI.Image>();
			cameraController = GameObject.Find ("CameraController").GetComponent<CameraController>();
		}

		// Update is called once per frame
		void Update () {
			if (!m_RegisteredFormat) {
				CameraDevice.Instance.SetFrameFormat (m_PixelFormat, false);
				if (CameraDevice.Instance.SetFrameFormat (m_PixelFormat, true)) m_RegisteredFormat = true;
			}
		}

		public void setRegisteredFormatFalse () {
			m_RegisteredFormat = false;
		}

		// Captures the image shown by the AR Camera.
		// Called when the user clicks on the Button "Take Picture". 
		public void TakeScreenShot() {

			// We take an instance of the Camera and the image stored in that instance. 
			Image image = CameraDevice.Instance.GetCameraImage (m_PixelFormat);

			if (image != null) {

				// Auxiliar Variables. 
				int w = image.Width;
				int h = image.Height;

				// Defintion of different Textures representing the different steps of the picture transformation.
				Texture2D tex = new Texture2D (w, h, TextureFormat.RGB24, false);
				Texture2D tex_rotated = new Texture2D (h, w, TextureFormat.RGB24, false);
				Texture2D tex_inverted = new Texture2D (h, w, TextureFormat.RGB24, false);
				Texture2D tex_final = new Texture2D (h, w, TextureFormat.RGB24, false);

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

				tex_final = Instantiate (tex_inverted);
				TextureScale.Bilinear (tex_final, Screen.width, Screen.height);

				float wCircle = selector.rectTransform.rect.width;
				float hCircle = selector.rectTransform.rect.height;
				Vector2 pos = selector.rectTransform.position;

				Color[] pix = tex_final.GetPixels ((int)(pos.x - wCircle/2), (int)(pos.y - hCircle/2), (int)wCircle, (int)hCircle);
				Texture2D tex_selected = new Texture2D ((int)wCircle, (int)hCircle);
				tex_selected.SetPixels (pix);
				tex_selected.Apply ();

				byte[] bytes = tex_selected.EncodeToPNG ();

				Destroy (tex);
				Destroy (tex_rotated);
				Destroy (tex_inverted);
				Destroy (tex_final);
				Destroy (tex_selected);

				// We send the bit-stream to the Camera Controller. 
				cameraController.PhotoTaked (bytes, Screen.width, Screen.height);

				//File.WriteAllBytes (Application.persistentDataPath + "/screen.png", bytes);

				m_RegisteredFormat = false;
			}
		}
	}
}