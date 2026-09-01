using BaseLib.Utils;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace ModRemoteScene.Examples;


/// <summary>
/// AddedNode example to show modded content within the context of the game scenes via remote scene tree.
/// </summary>
public class AddModImageToAllCards
{
    private static AddedNode<NCard, NImage> imageOnCard = new AddedNode<NCard, NImage>(nCard =>
    {
        var nImage = PreloadManager.Cache.GetScene("res://ModRemoteScene/image.tscn").Instantiate<NImage>();
        nCard.AddChildSafely(nImage);
        return nImage;
    });
}