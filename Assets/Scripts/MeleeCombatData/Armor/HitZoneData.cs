using System.Collections.Generic;
using System.Linq;

public class HitZoneData
{
    public static HitZoneData locationData = new HitZoneData();

    public enum HitLocationZone { 
        Head,Body,Legs
    }

    public HitLocationZone GetHitLocationZone(string location) {
        if (headUpper.Contains(location) || headLower.Contains(location))
            return HitLocationZone.Head;
        else if (legsUpper.Contains(location) || legsLower.Contains(location))
            return HitLocationZone.Legs;
        else
            return HitLocationZone.Body;
    }

    public List<string> headUpper;
    public List<string> headLower;
    public List<string> torsoUpper;
    public List<string> torsoLower;
    public List<string> legsUpper;
    public List<string> legsLower;
    public List<string> armsUpper;
    public List<string> armsLower;

    public HitZoneData()
    {
        List<string> bodyParts = new List<string>
        {
            "Forehead",
            "Eye",
            "Mouth",
            "Neck",
            "Base of Neck",
            "Shoulder Socket",
            "Shoulder Scapula",
            "Lung",
            "Heart",
            "Liver",
            "Stomach",
            "Stomach-Kidney",
            "Liver-Spine",
            "Liver-Kidney",
            "Intestines",
            "Spine",
            "Instetines-Pelvis",
            "Hip Socket",
            "Upper Arm",
            "Forearm",
            "Hand",
            "Thigh",
            "Shin",
            "Foot",
            "Knee",
            "Skull",
            "Face",
            "Throat",
            "Shoulder",
            "Upper Chest",
            "Lower Chest",
            "Pelvis"
        };

        headUpper = bodyParts.Where(part => part.Contains("Skull") || part.Contains("Forehead") || part.Contains("Eye")).ToList();
        headLower = bodyParts.Where(part => part.Contains("Mouth") || part.Contains("Neck") || part.Contains("Throat") || part.Contains("Face")).ToList();
        armsUpper = bodyParts.Where(part => part.Contains("Shoulder") || part.Contains("Upper Arm") 
            || part.Contains("Shoulder Socket") || part.Contains("Shoulder Scapula")).ToList();
        armsLower = bodyParts.Where(part => part.Contains("Forearm") || part.Contains("Hand")).ToList();
        legsUpper = bodyParts.Where(part => part.Contains("Thigh") || part.Contains("Hip Socket")).ToList();
        legsLower = bodyParts.Where(part => part.Contains("Shin") || part.Contains("Foot") || part.Contains("Knee")).ToList();
        torsoUpper = bodyParts.Where(part => part.Contains("Upper Chest") || part.Contains("Heart") || part.Contains("Lung")
            || part.Contains("Spine")).ToList();
        torsoLower = bodyParts.Where(part => part.Contains("Lower Chest") || part.Contains("Pelvis") || part.Contains("Stomach")
            || part.Contains("Spine") || part.Contains("Liver-Spine") || part.Contains("Liver-Kidney") || part.Contains("Liver")
            || part.Contains("Stomach-Kidney")).ToList();
    }
}
