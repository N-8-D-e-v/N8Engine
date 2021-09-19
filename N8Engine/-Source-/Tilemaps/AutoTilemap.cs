using N8Engine.Mathematics;
using static N8Engine.Mathematics.Pivot;

namespace N8Engine.Tilemaps
{
    public sealed class AutoTilemap<TPalette> where TPalette : TilePalette, new()
    {
        private readonly TPalette _palette;
        
        public TilemapBase GameObject { get; private set; }

        public AutoTilemap() => _palette = new TPalette();

        public AutoTilemap<TPalette> Place(Vector position, IntegerVector sizeInTiles, Pivot pivot)
        {
            var bottomLeft = position;
            var totalSize = sizeInTiles * _palette.TileSize;
            var positionAdjustedToPivot = bottomLeft.AdjustedToPivot(BottomLeft, totalSize, pivot);
            
            var chunk = new Chunk(positionAdjustedToPivot, sizeInTiles, _palette);
            chunk.GenerateTiles();
            GameObject = chunk.CreateCollider();
            return this;
        }
    }
}