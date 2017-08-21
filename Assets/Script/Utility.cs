using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
 

public static class Utility {

	public static string JsonToString( string target, string s){

		string[] newString = Regex.Split(target,s);

		return newString[1];

	}

	public static float JsonToFloat( string target, string s){

		string[] newString = Regex.Split(target,s);

		return float.Parse(newString[1]);

	}

	public static int JsonToInt(string target, string s){

		string[] newString = Regex.Split(target,s);

		return int.Parse(newString[1]);
	}

	public static bool JsonToBool(string target, string s){

		string[] newString = Regex.Split(target,s);

		int value = int.Parse (newString [1]);

		if (value == 0)
			return false;
		else
			return true;
	}

	public static Vector3 StringToVecter3(string target ){

		Vector3 newVector;
		string[] newString = Regex.Split(target,",");
		newVector = new Vector3( float.Parse(newString[0]), float.Parse(newString[1]), float.Parse(newString[2]));

		return newVector;
	}

	public static Vector2 StringToVecter2(string target ){

		Vector3 newVector;
		string[] newString = Regex.Split(target,",");
		newVector = new Vector2( float.Parse(newString[0]), float.Parse(newString[1]));

		return newVector;
	}

}
