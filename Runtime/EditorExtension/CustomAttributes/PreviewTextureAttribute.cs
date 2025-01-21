using System;
using UnityEngine;

namespace CHG.Utilities.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class PreviewTextureAttribute : PropertyAttribute
    {
        public const int ErrorMessageHeight = 25;


        public enum PropertyType {UNKNOWN, STRING, TEXTURE, TEXTURE2D, SPRITE, INVALID}
        
        #region States
        public string errorMessage;
        public bool isReady;
        public PropertyType propertyType;
        #endregion
        
        private Texture previewTexture;
        public Texture PreviewTexture
        {
            get {return previewTexture;}
            set {
                previewTexture = value;
                if(useTextureSize && value != null)
                {
                    width = previewTexture.width;
                    height = previewTexture.height;
                }
            }
        }


        public bool useTextureSize;
        public int width;
        public int height;
        public PreviewRenderType drawType;

        public PreviewTextureAttribute(PreviewRenderType drawType = PreviewRenderType.TRANSPARENT, bool useTextureSize = false, int width = 64, int height = 64)
        {
            this.drawType = drawType;
            this.useTextureSize = useTextureSize;

            this.width = width;
            this.height = height;
        }
    }
    
    /// <summary>
    /// Texture: Alpha 채널을 적용하지 않은 순수 Texture Render<br/>
    /// Transparent: 투명 값을 적용해서 Render(Default)<br/>
    /// Alpha: RGB + Alpha 채널을 합쳐서 Render
    /// </summary>
    public enum PreviewRenderType {TEXTURE, TRANSPARENT, ALPHA}
}