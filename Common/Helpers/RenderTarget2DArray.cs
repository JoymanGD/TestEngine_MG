using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.Helpers;

public class RenderTarget2DArray : RenderTarget2D
{
    public int Slices { get; }
    
    private List<RenderTarget2D> renderTargets = new ();
    
    public RenderTarget2DArray(GraphicsDevice graphicsDevice,
        int slices,
        int width,
        int height,
        bool mipMap,
        SurfaceFormat preferredFormat,
        DepthFormat preferredDepthFormat,
        int preferredMultiSampleCount,
        RenderTargetUsage usage,
        bool shared)
        : base(graphicsDevice, width, height, mipMap, preferredFormat, preferredDepthFormat, preferredMultiSampleCount, usage, shared, slices)
    {
        Slices = slices;

        for (int i = 0; i < Slices; i++)
        {
            RenderTarget2D Result = new RenderTarget2D(GraphicsDevice, Width, Height);
        
            renderTargets.Add(Result);
        }
    }

    public RenderTarget2D GetSlice(int id)
    {
        if (id >= Slices)
        {
            throw new ArgumentException("Slice id is out of range", nameof (id));
        }

        if (renderTargets.Count > id)
        {
            return renderTargets[id];
        }
        
        RenderTarget2D Result = new RenderTarget2D(GraphicsDevice, Width, Height, false, SurfaceFormat.Single, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents, false);
        
        renderTargets.Add(Result);

        return Result;
    }

    public void Prepare()
    {
        for (int i = 0; i < Slices; i++)
        {
            RenderTarget2D renderTarget2D = renderTargets[i];
            var elementCount = Width * Height;
            Color[] renderTargetData = new Color[elementCount];
            renderTarget2D.GetData(renderTargetData);

            var rect = new Rectangle(0, 0, Width, Height);
            
            SetData(0, i, rect, renderTargetData, 0, elementCount);
            
            // renderTarget2D.CopyData(this, 0, i, 0, 0, Width, Height, 0, 0, 0, 0);
        }
    }
}
