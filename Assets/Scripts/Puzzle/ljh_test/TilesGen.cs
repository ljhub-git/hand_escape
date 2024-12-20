using UnityEngine;

public class TilesGen : MonoBehaviour
{
    public string imageFilename;
    private Texture2D mTextureOriginal;

    private Tile mTile = new Tile();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateBaseTexture();

        mTile.DrawCurve(Tile.Direction.UP, Tile.PosNegType.POS, Color.blue);
        mTile.DrawCurve(Tile.Direction.RIGHT, Tile.PosNegType.POS, Color.blue);
        mTile.DrawCurve(Tile.Direction.DOWN, Tile.PosNegType.POS, Color.blue);
        mTile.DrawCurve(Tile.Direction.LEFT, Tile.PosNegType.POS, Color.blue);

        mTile.DrawCurve(Tile.Direction.UP, Tile.PosNegType.NEG, Color.red);
        mTile.DrawCurve(Tile.Direction.RIGHT, Tile.PosNegType.NEG, Color.red);
        mTile.DrawCurve(Tile.Direction.DOWN, Tile.PosNegType.NEG, Color.red);
        mTile.DrawCurve(Tile.Direction.LEFT, Tile.PosNegType.NEG, Color.red);
    }

    void CreateBaseTexture()
    {
        mTextureOriginal = SpriteUtils.LoadTexture(imageFilename);
        if(!mTextureOriginal.isReadable)
        {
            Debug.Log("Texutre is nor readable");
            return;
        }

        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteUtils.CreateSpriteFromTexture2D(
            mTextureOriginal,
            0,
            0,
            mTextureOriginal.width,
            mTextureOriginal.height);
    }

    private (Tile.PosNegType, Color) GetRendomType()
    {
        Tile.PosNegType type = Tile.PosNegType.POS;
        Color color = Color.blue;
        float rand = Random.Range(0f, 1f);

        if(rand < 0.5f)
        {
            type = Tile.PosNegType.POS;
            color = Color.blue;
        }
        else
        {
            type = Tile.PosNegType.NEG;
            color = Color.red;
        }
        return (type, color);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mTile.HideAllCurves();

            var type_color = GetRendomType();
            mTile.DrawCurve(Tile.Direction.UP, type_color.Item1, type_color.Item2);
            type_color = GetRendomType();
            mTile.DrawCurve(Tile.Direction.RIGHT, type_color.Item1, type_color.Item2);
            type_color = GetRendomType();
            mTile.DrawCurve(Tile.Direction.DOWN, type_color.Item1, type_color.Item2);
            type_color = GetRendomType();
            mTile.DrawCurve(Tile.Direction.LEFT, type_color.Item1, type_color.Item2);
        }
    }
}
