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

		private CameraController cameraController;

		// Variables for the AR Camera configuration. 
		private Image.PIXEL_FORMAT m_PixelFormat = Image.PIXEL_FORMAT.RGB888;
		private bool m_RegisteredFormat = false;

		// Functions executed on the intilization of the application. 
		void Start() {

			CameraDevice.Instance.SetFocusMode( 
				CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

			cameraController = GameObject.Find ("CameraController").GetComponent<CameraController>();
		}

		// Update is called once per frame
		void Update () {

			CameraDevice.Instance.SetFocusMode( 
				CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

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

			// We send the bit-stream to the Camera Controller. 
			cameraController.scanActualImage(image);

			m_RegisteredFormat = false;
		}
	}
}