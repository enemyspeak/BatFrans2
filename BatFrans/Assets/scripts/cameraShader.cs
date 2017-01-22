using System;
using UnityEngine;


[ExecuteInEditMode]
public class cameraShader : MonoBehaviour//UnityStandardAssets.ImageEffects.ImageEffectBase
{
	public Material material;

	[SerializeField] RenderTexture mainSceneRT;
	[SerializeField] RenderTexture scaledRT;

	public int horizontalResolution = 320;
	public int verticalResolution = 240;

	void Start () {
	}
//
	void OnPreRender() {
		this.GetComponent<Camera>().targetTexture = mainSceneRT;
		Graphics.Blit (mainSceneRT, null, material);

//		// this ensures that w/e the camera sees is rendered to the above RT
	}
//
//	void OnPostrender() {
//		Graphics.Blit(rayMarchRT, mainSceneRT,material);
//		// You have to set target texture to null for the Blit below to work
////		this.GetComponent<Camera>().targetTexture = null;
//		Graphics.Blit(mainSceneRT, null as RenderTexture);
//	}
//
//	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		// To draw the shader at full resolution, use: 
		Graphics.Blit (mainSceneRT, null, material);

//		// To draw the shader at scaled down resolution, use:
//		RenderTexture scaledRT = RenderTexture.GetTemporary (horizontalResolution, verticalResolution);
//		Graphics.Blit (mainSceneRT, scaledRT, material);
//		Graphics.Blit (scaledRT, mainSceneRT);
////		Graphics.Blit(mainSceneRT, null as RenderTexture);
//		RenderTexture.ReleaseTemporary (scaledRT);
	}
}