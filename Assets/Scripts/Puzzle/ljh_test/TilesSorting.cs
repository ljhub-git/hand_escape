using System.Collections.Generic;
using UnityEngine;

public class TilesSorting
{
    private List<MeshRenderer> mSortIndices = new List<MeshRenderer>();

    public TilesSorting()
    {
        mSortIndices.Clear();
    }

    public void Add(MeshRenderer renderer)
    {
        mSortIndices.Add(renderer);
        SetRenderOrder(renderer, mSortIndices.Count);
    }

    public void Remove(MeshRenderer renderer) 
    {
        mSortIndices.Remove(renderer);
        for(int i = 0; i< mSortIndices.Count; i++)
        {
            SetRenderOrder(mSortIndices[i], i + 1);
        }
    }

    public void BringToTop(MeshRenderer renderer)
    {
        Remove(renderer);
        Add(renderer);
    }

    private void SetRenderOrder(MeshRenderer renderer, int index)
    {
        renderer.sortingOrder = index;
        Vector3 p = renderer.transform.position;
        p.z = -index / 10.0f;
        renderer.transform.position = p;
    }
}
