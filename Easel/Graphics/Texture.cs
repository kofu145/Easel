﻿using System;
using Easel.Math;
using Easel.Scenes;
using Pie;

namespace Easel.Graphics;

/// <summary>
/// The base texture class, for objects that can be textured.
/// </summary>
public abstract class Texture : IDisposable
{
    /// <summary>
    /// Returns <see langword="true"/> if this <see cref="Texture"/> has been disposed.
    /// </summary>
    public bool IsDisposed { get; protected set; }
    
    /// <summary>
    /// The native Pie <see cref="Pie.Texture"/>.
    /// </summary>
    public Pie.Texture PieTexture { get; protected set; }

    /// <summary>
    /// The size (resolution), in pixels of the texture.
    /// </summary>
    public Size Size => (Size) PieTexture.Size;

    protected Texture(bool autoDispose)
    {
        if (autoDispose)
            SceneManager.ActiveScene.GarbageCollections.Add(this);
    }

    public virtual void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;
        PieTexture.Dispose();
        Logging.Log("Texture disposed.");
    }
}