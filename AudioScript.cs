using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class AudioScript : MonoBehaviour
{
   private AudioClip[] audioArray;
   public Transform PlayerPosition;
   [SerializeField]
   private GameObject[] SoundZones; //GameObject name MUST be the same as the sound file it represents

   private AudioSource audio;
   private AudioClip currentSound;
   
   public float SoundZoneRadius = 5.0f; //The radius of ALL sound zones
   // ref to the size  1 = 1 square on the grid 
   private Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();
   // private string[] getAllAudioFileNames = Directory.GetFiles(@"\Unity\Card_game\Assets\Resources\audio", "*.wav"); //Get all audio file names inside the folder DO NOT USE THIS < -- android complies down to WWW method
   // and requires persistent data path to load files in to memory

   // private void Awake()
   // {
   //    Object[] getAllAudioFileNamesTest = Resources.LoadAll("audio", typeof(AudioClip));
   //    foreach (var t in getAllAudioFileNamesTest)
   //    {
   //       Debug.Log(t.name);
   //    }
   // }


   private void Start()
   {
      Object[] getAllAudioFileNames = Resources.LoadAll("audio", typeof(AudioClip));
      audioArray = Resources.LoadAll<AudioClip>("audio"); //Load all audio clips in to audioArray, array
      var ittareOverAudioFileArray = 0;
      foreach (var file in getAllAudioFileNames) //making a dictionary Were audioArray == value, getAllAudioFileNames == key
      {
         audioDictionary.Add(Path.GetFileName(file.name), audioArray[ittareOverAudioFileArray]);
         ittareOverAudioFileArray++;
      }
      audio = gameObject.AddComponent<AudioSource>();
      audio.clip = audioDictionary[FindClosestSoundZone(PlayerPosition).name];
      audio.Play();
      currentSound = audio.clip;
   }

   public IEnumerator PlaySound()
   {
      AudioClip chosenSound = audioDictionary[FindClosestSoundZone(PlayerPosition).name];
      if (chosenSound == currentSound)
      {
         yield break;
      }
      Destroy(this.GetComponent<AudioSource>());
      audio = gameObject.AddComponent<AudioSource>();
      audio.clip = chosenSound;
      audio.Play();
      currentSound = audio.clip;
      yield return new WaitForEndOfFrame();
   }

   /* measure the distance between player and every GameObject inside SoundZones array store the values inside distanceList
    then  iterate over the list to get index of the lowest value and use that to return the GameObject that is closest to the player
     IF the player is not inside any of the zones return default GameObject but never NULL*/
   private GameObject FindClosestSoundZone(Transform currentPlayerPosition)
   {
      List<double> distanceList = new List<double>();
      foreach (var soundZoneCenter in SoundZones)
      {
         var distance = Math.Sqrt(Math.Pow((soundZoneCenter.transform.position.x - currentPlayerPosition.position.x), 2) +
                   Math.Pow((soundZoneCenter.transform.position.y - currentPlayerPosition.position.y), 2));
         distanceList.Add(distance);
      }

      var lowestNumberIndex = 0;
      for (int distanceListIterral = 0; distanceListIterral < distanceList.Count; distanceListIterral++)
      {
         if (distanceList[distanceListIterral] < distanceList[lowestNumberIndex])
         {
            lowestNumberIndex = distanceListIterral;
         }
      }

      return distanceList[lowestNumberIndex] <= SoundZoneRadius ? SoundZones[lowestNumberIndex] : SoundZones[0];
   }
}
//
// String path= @"Y:\\Test\\Project\\bin\\Debug";
// String[] extract = Regex.Split(path,"bin");  //split it in bin
// String main = extract[0].TrimEnd('\\'); //extract[0] is Y:\\Test\\Project\\ ,so exclude \\ here
// Console.WriteLine("Main Path: "+main);//get main path
