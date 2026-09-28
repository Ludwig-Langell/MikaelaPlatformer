using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoURL : MonoBehaviour
{
    public VideoPlayer player;

    void Start()
    {
        player.source = VideoSource.Url;
        player.url = Path.Combine(Application.streamingAssetsPath, "intro.mp4");
        player.Prepare();
        player.prepareCompleted += _ => player.Play();
        player.loopPointReached += _ => OnVideoDone();
//laddar in videon så den spelas på exakt rätt tillfälle
    }

    void OnVideoDone()
    {
        //nästa scen laddas in
    }
}

//detta script skapades då det var problem med att introvideo ej spelades upp på andra enheter, förmodligen pga att man inte kommer åt filen, så gör om video source till url