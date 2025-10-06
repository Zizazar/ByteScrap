using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.TextCore;

[CustomEditor(typeof(TMP_SpriteAsset))]
public class ResetSpriteBX : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TMP_SpriteAsset spriteAsset = (TMP_SpriteAsset)target;

        if (GUILayout.Button("Reset All BX to Zero"))
        {
            if (spriteAsset.spriteCharacterTable != null)
            {
                foreach (var spriteCharacter in spriteAsset.spriteCharacterTable)
                {
                    // Доступ к глифу спрайта и его настройкам
                    var spriteGlyph = spriteCharacter.glyph;
                    var newMetrics = new GlyphMetrics
                    {
                        horizontalBearingX = 0,
                        horizontalAdvance = spriteGlyph.metrics.horizontalAdvance,
                        horizontalBearingY = spriteGlyph.metrics.horizontalBearingY,
                        height = spriteGlyph.metrics.height,
                        width = spriteGlyph.metrics.width
                    };
                    spriteGlyph.metrics = newMetrics;
                    EditorUtility.SetDirty(spriteAsset);
                }
                AssetDatabase.SaveAssets();
                Debug.Log("Reset all BX values to 0 for " + spriteAsset.name);
            }
        }
    }
}