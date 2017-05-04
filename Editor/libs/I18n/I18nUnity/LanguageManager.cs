using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mgl;

public class LanguageManager {
	private static I18n _i18n = I18n.Instance;
	//public static I18n Language;
	private static bool _initLanguage = false;
	public static bool IsInitialized {
		get { return _initLanguage; }	
	}

	// Change the path later
	public static void Initialize(){
		// Change the local path later
		//string localePath = "Assets/DungeonGenerator/Editor/libs/I18n/Method2/I18nUnity/Locales/";
		if (!_initLanguage){
			I18n.Configure();	
			_initLanguage = true;
		}
		// Can't change the language here
	}

	public static string GetText(string key, params object[] args){
		// for debug purpose
		string text = _i18n.__(key, args);
		//Debug.Log("Key: " + key + ", Text: " + text + ", Language: " + I18n.GetLocale()+".");
		return text;
	}

	public static string GetLanguage(){
		return I18n.GetLocale();
	}

	public static void SetLanguage(string language){
		I18n.SetLocale(language);
	}

	public static void SetPath(string path){
		I18n.SetPath(path);
	}
}
