using System;
using UnityEngine;


[ExecuteInEditMode]
public class cameraShader : MonoBehaviour//UnityStandardAssets.ImageEffects.ImageEffectBase
{
	public Material material;
	public Texture source;
	public RenderTexture destination;

	public int horizontalResolution = 320;
	public int verticalResolution = 240;

	void Start () {
	}

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		// To draw the shader at full resolution, use: 
		// Graphics.Blit (source, destination, material);

		// To draw the shader at scaled down resolution, use:
		RenderTexture scaled = RenderTexture.GetTemporary (horizontalResolution, verticalResolution);
		Graphics.Blit (source, scaled, material);
		Graphics.Blit (scaled, destination);
		RenderTexture.ReleaseTemporary (scaled);
	}
}