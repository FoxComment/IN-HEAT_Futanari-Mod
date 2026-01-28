using FutanariMod;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppMonsterBox.Runtime.Extensions._Localization;
using Il2CppMonsterBox.Runtime.Gallery;
using Il2CppMonsterBox.Runtime.Gameplay.Arcade;
using Il2CppMonsterBox.Runtime.Gameplay.Arcade.Character;
using Il2CppMonsterBox.Runtime.Gameplay.Arcade.Level;
using Il2CppMonsterBox.Runtime.Gameplay.Character;
using Il2CppMonsterBox.Runtime.Gameplay.Enums;
using Il2CppMonsterBox.Runtime.Gameplay.Level;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Character;
using Il2CppMonsterBox.Runtime.Gameplay.Scene.Adult;
using Il2CppMonsterBox.Runtime.Gameplay.SecurityOffice;
using Il2CppMonsterBox.Runtime.Gameplay.SecurityOffice.Character;
using Il2CppMonsterBox.Runtime.UI.Gallery.Character;
using Il2CppMonsterBox.Runtime.UI.Game.Adult;
using Il2CppMonsterBox.Runtime.UI.Game.Arcade;
using Il2CppMonsterBox.Runtime.UI.Game.Result;
using Il2CppMonsterBox.Runtime.UI.Settings;
using Il2CppMonsterBox.Systems.Saving;
using Il2CppMonsterBox.Systems.SceneManagement;
using Il2CppMonsterBox.Systems.UI.Elements;
using Il2CppTMPro;
using MelonLoader;
using MelonLoader.Utils;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
//using UnityEngine.Animations;
//using Il2CppMonsterBox.Systems.Tools.Miscellaneous.Gameplay;

[assembly: MelonInfo(typeof(ModMain), "Futanari Mod", "2026.1.25", "FoxComment", "https://github.com/foxcomment/IN-HEAT_Futanari-Mod")]
[assembly: MelonGame("MonsterBox", "IN HEAT")]
namespace FutanariMod
{
	public class ModMain : MelonMod
	{




		#region Vars




		public static List<Appendage> ActiveAppendages = new(0);
		public struct Appendage
		{
			public GameObject _GameObject;
			public Character _Character;
			public Animator _Animator;
			public Appendage(GameObject _gameobject, Character _character, Animator _animator)
			{
				_GameObject = _gameobject;
				_Character = _character;
				_Animator = _animator;
			}
		}

		private const string lazyAddressField = "FoxCom-Futanari-AB-v3";

		private const string file_AppendageSammyPrefab = "Assets/Futa/Prefs/AppendageSammy.prefab";
		private const string file_AppendageMaddiePrefab = "Assets/Futa/Prefs/AppendageMaddie.prefab";
		private const string file_AppendagePoppiPrefab = "Assets/Futa/Prefs/AppendagePoppi.prefab";
		private const string file_AppendageNilePrefab = "Assets/Futa/Prefs/AppendageNile.prefab";
		private const string file_AppendageAriPrefab = "Assets/Futa/Prefs/AppendageAri.prefab";
		private const string file_AppendageMistyPrefab = "Assets/Futa/Prefs/AppendageMisty.prefab";
		private const string file_AppendageSammySlimePrefab = "Assets/Futa/Prefs/AppendageSammySlime.prefab";
		private const string file_AppendageSammyLowPolyPrefab = "Assets/Futa/Prefs/AppendageLowPoly.prefab";


		/// <summary> 0 = SO, 1 = NC, 2 = ARC, 3 = LP.  / 023</summary>
		private readonly string[] file_ArcadeCGSammySprite = new string[4] { "Assets/Futa/Sprites/CG_Arc_SammySO.png", "", "Assets/Futa/Sprites/CG_Arc_SammyA.png", "Assets/Futa/Sprites/CG_Arc_SammyLowPoly.png" };
		/// <summary> 0 = SO, 1 = NC, 2 = ARC.   /1</summary>
		private readonly string[] file_ArcadeCGMaddieSprite = new string[3] { "", "Assets/Futa/Sprites/CG_Arc_MaddieNC.png", "" };
		/// <summary> 0 = SO, 1 = NC, 2 = ARC.   /0</summary>
		private readonly string[] file_ArcadeCGPoppiSprite = new string[3] { "Assets/Futa/Sprites/CG_Arc_PoppiSO.png", "", "" };
		/// <summary> 0 = SO, 1 = NC, 2 = ARC.   /2</summary>
		private readonly string[] file_ArcadeCGAriSprite = new string[3] { "", "", "Assets/Futa/Sprites/CG_Arc_AriA.png" };
		/// <summary> 0 = SO, 1 = NC, 2 = ARC.   /0</summary>
		private readonly string[] file_ArcadeCGMistySprite = new string[3] { "Assets/Futa/Sprites/CG_Arc_CisSO.png", "", "" };


		private const string file_CreamScreenPrefab = "Assets/Futa/Prefs/CreamScreen.prefab";
		private const string file_FutaGalleryOptionsPrefab = "Assets/Futa/Prefs/Futa Gallery.prefab";
		private const string file_ModLogoPrefab = "Assets/Futa/Prefs/ModLog.prefab";
		//private const string animTestFile = "Assets/Futa/Testing/TestN.controller";

		private static List<CharacterControllerBase> activeCharactersGameplay;
		private static List<GalleryCharacter> activeCharactersGallery;

		//RuntimeAnimatorController animTEST;
		//RuntimeAnimatorController animPlace;

		private GameObject prefabModLogo;
		private GameObject prefabCreamScreen;
		private GameObject prefabFutaGallerySettings;

		private GameObject activeCreamScreen;
		private GameObject activeFutanariGallerySettings;

		private GameObject prefabAppendageAri;
		private GameObject prefabAppendageMaddie;
		private GameObject prefabAppendageMisty;
		private GameObject prefabAppendageNile;
		private GameObject prefabAppendagePoppi;
		private GameObject prefabAppendageSammy;
		private GameObject prefabAppendageSammySlime;
		private GameObject prefabAppendageSammyLowPoly;

		/// <summary> 0 = SO, 1 = NC, 2 = ARC, 3 = LP.  / 023</summary>
		private Sprite[] arcadeGOSammy = new Sprite[4] { null, null, null, null };
		/// <summary> 0 = SO, 1 = NC, 2 = ARC.   /1</summary>
		private Sprite[] arcadeGOMaddie = new Sprite[3] { null, null, null };
		/// <summary> 0 = SO, 1 = NC, 2 = ARC.   /0</summary>
		private Sprite[] arcadeGOPoppi = new Sprite[3] { null, null, null };
		/// <summary> 0 = SO, 1 = NC, 2 = ARC.   /2</summary>
		private Sprite[] arcadeGOAri = new Sprite[3] { null, null, null };
		/// <summary> 0 = SO, 1 = NC, 2 = ARC.   /0</summary>
		private Sprite[] arcadeGOMisty = new Sprite[3] { null, null, null };

		Shader defaultShader;
		Shader slimeShader;
		Image arcadeBackgroundLoseAppendageLayer;

		private RectTransform adultSection;
		private static ModMain Instance;
		private Slider excitementSliderGallery;
		private Toggle leakToggleGallery;

		public Button creamButton { get; private set; }

		GameObject modLogo = null;
		LevelManagerBase levelManager;
		GalleryManager galleryManager;

		bool creaming = true;

		public static bool Adult { get; private set; } = true;
		public static bool Topless { get; private set; } = false;
		public static bool Intersex { get; private set; } = false;

		/// <summary>
		/// Not sure where to obtain info on an attacking character properly tbh, so heres the workaroundd
		/// </summary>
		static Character attackedCharacter;

		static AssetBundleCreateRequest RequestAB;
		static AssetBundle Bundle;


		float galleryRotationOffset;




		#endregion




		#region Initialisation




		public override void OnLateInitializeMelon() => LoadAssetBundle();



		/// <summary>
		/// MIGHT be usefull later.
		/// </summary>
		public static byte[] GetBytesFromStream(Stream _stream)
		{
			byte[] byes = new byte[_stream.Length];
			_stream.Read(byes, 0, byes.Length);
			_stream.Dispose();
			return byes;
		}



		static GameObject UnpackAssetGameObject(string _address)
		{
			AssetBundleRequest _assetTMP = Bundle.LoadAssetAsync<GameObject>(_address);

			return _assetTMP.asset.Cast<GameObject>();
		}


		static Sprite UnpackAssetSprite(string _address)
		{
			AssetBundleRequest _assetTMP = Bundle.LoadAssetAsync<Sprite>(_address);

			return _assetTMP.asset.Cast<Sprite>();
		}



		static RuntimeAnimatorController UnpackAssetAnimator(string _address)
		{
			AssetBundleRequest _assetTMP = Bundle.LoadAssetAsync<RuntimeAnimatorController>(_address);

			return _assetTMP.asset.Cast<RuntimeAnimatorController>();
		}



		/// <summary>
		/// Check if asset bundle file is present
		/// </summary>
		bool CheckAssetBundlePresence(bool _beep = false)
		{
			if (!File.Exists(MelonEnvironment.ModsDirectory + "\\" + lazyAddressField))
			{
				MelonLogger.Msg("\n \n \n");
				MelonLogger.BigError("FUTANARI MOD", "Please, put the '" +
				lazyAddressField + "' file into the mods folder\n \n" +
				"No asset bundle found at " + MelonEnvironment.ModsDirectory + "\\" +
				"\n\nAsset bundle contains all the custom stuff that loads on game launch.");
				MelonLogger.Msg("\n \n \n");

				if (_beep)
					Console.Beep();

				return false;
			}

			return true;
		}



		private void LoadAssetBundle()
		{
			if (!CheckAssetBundlePresence(true))
				return;

			Assembly assembly = MelonAssembly.Assembly;
			RequestAB = AssetBundle.LoadFromFileAsync(MelonEnvironment.ModsDirectory + "\\" + lazyAddressField);
			Bundle = RequestAB.assetBundle;

			MelonLogger.Warning("\n\n\nPlease, make sure that you're using the right version of '" + lazyAddressField +
			"' from the same download as the .dll file itself.\nOtherwise you Will get a bunch of errors and a potential game crash.\n\n\n");

			prefabAppendageSammy = UnpackAssetGameObject(file_AppendageSammyPrefab);
			prefabAppendageSammy.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageSammySlime = UnpackAssetGameObject(file_AppendageSammySlimePrefab);
			prefabAppendageSammySlime.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageAri = UnpackAssetGameObject(file_AppendageAriPrefab);
			prefabAppendageAri.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageMisty = UnpackAssetGameObject(file_AppendageMistyPrefab);
			prefabAppendageMisty.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendagePoppi = UnpackAssetGameObject(file_AppendagePoppiPrefab);
			prefabAppendagePoppi.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageNile = UnpackAssetGameObject(file_AppendageNilePrefab);
			prefabAppendageNile.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageMaddie = UnpackAssetGameObject(file_AppendageMaddiePrefab);
			prefabAppendageMaddie.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageSammyLowPoly = UnpackAssetGameObject(file_AppendageSammyLowPolyPrefab);
			prefabAppendageSammyLowPoly.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabCreamScreen = UnpackAssetGameObject(file_CreamScreenPrefab);
			prefabCreamScreen.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabFutaGallerySettings = UnpackAssetGameObject(file_FutaGalleryOptionsPrefab);
			prefabFutaGallerySettings.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabModLogo = UnpackAssetGameObject(file_ModLogoPrefab);
			prefabModLogo.hideFlags |= HideFlags.DontUnloadUnusedAsset;



			arcadeGOSammy[0] = UnpackAssetSprite(file_ArcadeCGSammySprite[0]);
			arcadeGOSammy[0].hideFlags |= HideFlags.DontUnloadUnusedAsset;

			arcadeGOSammy[2] = UnpackAssetSprite(file_ArcadeCGSammySprite[2]);
			arcadeGOSammy[2].hideFlags |= HideFlags.DontUnloadUnusedAsset;

			arcadeGOSammy[3] = UnpackAssetSprite(file_ArcadeCGSammySprite[3]);
			arcadeGOSammy[3].hideFlags |= HideFlags.DontUnloadUnusedAsset;

			arcadeGOAri[2] = UnpackAssetSprite(file_ArcadeCGAriSprite[2]);
			arcadeGOAri[2].hideFlags |= HideFlags.DontUnloadUnusedAsset;

			arcadeGOMisty[0] = UnpackAssetSprite(file_ArcadeCGMistySprite[0]);
			arcadeGOMisty[0].hideFlags |= HideFlags.DontUnloadUnusedAsset;

			arcadeGOPoppi[0] = UnpackAssetSprite(file_ArcadeCGPoppiSprite[0]);
			arcadeGOPoppi[0].hideFlags |= HideFlags.DontUnloadUnusedAsset;

			arcadeGOMaddie[1] = UnpackAssetSprite(file_ArcadeCGMaddieSprite[1]);
			arcadeGOMaddie[1].hideFlags |= HideFlags.DontUnloadUnusedAsset;



			//animTEST = UnpackAssetAnimation(animTestFile);

			MelonLogger.Msg(MelonLoader.Logging.ColorARGB.Lime, lazyAddressField + " is loaded.");

			//animTEST = UnpackAssetAnimator(animTestFile);
			//animTEST.hideFlags |= HideFlags.DontUnloadUnusedAsset;
		}



		private void InstantiateModLogo()
		{
			modLogo = UnityEngine.Object.Instantiate(prefabModLogo);
			modLogo.transform.localScale = Vector3.one * .1f;
			modLogo.transform.position = new Vector3(-.029f, .49f, .45f);
			modLogo.transform.rotation = Quaternion.Euler(0, 193.435f, 0.02f);
		}



		private void FetchDefaultAssets()
		{
			Adult = SaveManager.LoadSettings().HContent;

			ApplyAdultToggles();

			if (defaultShader)
				return;

			if (!GameObject.FindObjectOfType<SkinnedMeshRenderer>())
				return;

			defaultShader = GameObject.FindObjectOfType<SkinnedMeshRenderer>().material.shader;
			defaultShader.hideFlags |= HideFlags.DontUnloadUnusedAsset;
		}



		private static void CheckSettingsPresence()
		{
			if (!PlayerPrefs.HasKey("Intersex"))
				PlayerPrefs.SetString("Intersex", "True");

			if (!PlayerPrefs.HasKey("Topless"))
				PlayerPrefs.SetString("Topless", "False");
		}




		public override void OnSceneWasInitialized(int buildindex, string sceneName)
		{
			if (!CheckAssetBundlePresence())
				return;

			Instance = this;

			activeCharactersGameplay = new(0);
			activeCharactersGallery = new(0);
			ActiveAppendages = new(0);

			if (Instance.activeFutanariGallerySettings)
				UnityEngine.Object.Destroy(Instance.activeFutanariGallerySettings);

			if (Instance.activeCreamScreen)
				UnityEngine.Object.Destroy(Instance.activeCreamScreen);

			if (modLogo)
				UnityEngine.Object.Destroy(Instance.modLogo);

			creaming = false;

			if (LevelManagerBase.Exists)
				levelManager = LevelManagerBase.Instance;

			if (GalleryManager.Exists)
			{
				galleryManager = GalleryManager.Instance;
				activeFutanariGallerySettings = UnityEngine.Object.Instantiate(Instance.prefabFutaGallerySettings, GameObject.Find("Container - Left").transform);   //Spawm Splooge screen
				excitementSliderGallery = activeFutanariGallerySettings.GetComponentInChildren<Slider>();
				excitementSliderGallery.onValueChanged.AddListener(new Action<float>((_s) => { CharacterGalleryApplyFutaSettings(_s); }));                  //Add listener to button presses

				leakToggleGallery = activeFutanariGallerySettings.GetComponentInChildren<Toggle>();
				leakToggleGallery.onValueChanged.AddListener(new Action<bool>((_t) => { CharacterGalleryApplyFutaSettings(_t); }));
			}

			CheckSettingsPresence();
			FetchDefaultAssets();


			if (sceneName.Contains("Title"))
				InstantiateModLogo();
		}




		#endregion




		#region Appendage




		public void InstantiateAppendage(Character _character, Animator _c_anim, Transform _hipBone)
		{
			GameObject _appendage;
			Character _charEnum;

			if ((int)_character >= 1000 && _character != Character.SammyGoon)
				_charEnum = (Character)((int)_character / 1000);
			else
				_charEnum = _character;


			switch (_charEnum)   //Instntiate a speciffic appendage model
			{
				case Character.Ari:    //Canine
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageAri, _hipBone);
					break;

				case Character.Misty:  //Tapered
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageMisty, _hipBone);
					break;

				case Character.Sammy:  //Humanoid
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageSammy, _hipBone);
					break;

				case Character.Poppi:  //Humanoid
					_appendage = UnityEngine.Object.Instantiate(prefabAppendagePoppi, _hipBone);
					break;

				case Character.Maddie:  //Thick
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageMaddie, _hipBone);
					break;

				case Character.Nile:  //Humanoid
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageNile, _hipBone);
					break;

				case Character.SammyGoon:

					_appendage = UnityEngine.Object.Instantiate(prefabAppendageSammySlime, _hipBone);
					break;
				default:    //Generic
							//Better to leave it empty, because it had created an appendage on a dor for whatever reason~
					return;
			}

			SkinnedMeshRenderer _mesh = _appendage.transform.GetComponentInChildren<SkinnedMeshRenderer>();
			Animator _a_anim = _appendage.GetComponent<Animator>();

			if (_character == Character.SammyGoon)
			{
				if (!slimeShader)
				{
					slimeShader = _c_anim.GetComponentInChildren<SkinnedMeshRenderer>().material.shader;

					slimeShader.hideFlags |= HideFlags.DontUnloadUnusedAsset;
				}

				_mesh.material.shader = slimeShader;
			}
			else
				_mesh.material.shader = defaultShader;

			switch (_charEnum)
			{
				case Character.Ari:
					SetupAppendage(_a_anim, .103f, .49f, 1, 1);
					break;  //Green

				case Character.SammyGoon:
					SetupAppendage(_a_anim, .10f, .34f, 1, .8f);
					break;  //Tiger Slime

				case Character.Sammy:
					SetupAppendage(_a_anim, .065f, .68f, .3f, .16f);
					break;  //Tiger

				case Character.Poppi:
					SetupAppendage(_a_anim, .075f, .5f, 0, .1f);
					break;  //Buni

				case Character.Nile:
					//animPlace = _c_anim.runtimeAnimatorController;
					//_c_anim.runtimeAnimatorController = animTEST;
					SetupAppendage(_a_anim, .091f, .55f, 0, .45f);
					break;  //Black

				case Character.Maddie:
					SetupAppendage(_a_anim, .2f, .65f, .7f, .5f);
					break;  //Burger

				case Character.Misty:
					SetupAppendage(_a_anim, .1f, .5f, 1);
					break;  //Shark

				case Character.Kass:
					UnityEngine.Object.Destroy(_appendage);
					return; //Kass is not an actual enemy character (racoon dumpster thing)

				case Character.AriBM:
					UnityEngine.Object.Destroy(_appendage);
					return; //Ask yourself if this one needs an appendage.
			}

			MelonCoroutines.Start(AppendageLifeCycle(_charEnum, _c_anim, _mesh));   //Start appendage routine for a fresh new appendage
		}



		/// <summary>
		/// Set Placement, Scale and overall look.
		/// </summary>
		/// <param name="_anim">Animator</param>
		/// <param name="_offest">How much the appendage should be moved forward (locally)</param>
		/// <param name="_scale">Scale of appendage's GameObject</param>
		/// <param name="_fertility">Determens the size of Two Male Bump. Removes them completely if 0</param>
		/// <param name="_diameter">Additional diameter of an appendage (stacks with _scale)</param>
		private void SetupAppendage(Animator _anim, float _offest = .068f, float _scale = .65f, float _diameter = .3f, float _fertility = 0)
		{
			_anim.transform.localRotation = Quaternion.Euler(0, 0, 0);  //Fiх any random tilt
			_anim.transform.localPosition = Vector3.forward * _offest;
			_anim.transform.localScale = Vector3.one * _scale;
			_anim.SetFloat("Fertility", _fertility);
			_anim.SetFloat("Thickness", _diameter);
		}



		/// <summary>
		/// Appendage routine
		/// </summary>
		/// <param name="_controller">Assigned character controller</param>
		private IEnumerator AppendageLifeCycle(Character _character, Animator _c_anim, SkinnedMeshRenderer _a_mesh)
		{
			Animator _a_anim = _a_mesh.transform.parent.GetComponent<Animator>();

			_a_anim.SetFloat("Breathe", UnityEngine.Random.value * 3);                  //Desync clenching speed (Makes a suttle "throb" animation, which is kinda nice)
			_a_anim.SetBool("Intersex", Intersex && SaveManager.Settings.HContent);     //Assign current settings
			_a_anim.SetBool("Topless", Topless);                                        //Assign current settings

			ActiveAppendages.Add(new Appendage(_a_mesh.transform.parent.gameObject, _character, _a_anim));

			if (galleryManager)                                                         //Hide appendage in gallery, cuz deh characters are dressed by default.
			{
				_a_anim.gameObject.SetActive(false);
				yield break;
			}

			switch (LevelManagerBase.Level)                                             //Gameplay appendage behavieor
			{
				case Level.SecurityOffice:

					_a_anim.SetFloat("Enlarge", .65f);
					break;

				case Level.Nightclub:

					NightclubCharacterController _clubController = _c_anim.GetComponentInParent<NightclubCharacterController>();
					while (_a_mesh)                                       //Cycle while appendage eхists
					{
						_a_anim.SetFloat("Enlarge", _clubController._excitement._excitement);
						_a_anim.SetBool("Leak", _clubController.IsExcited);

						yield return new WaitForSeconds(.2f);           //Artificial value, can be whatever, but .2 looks smoothest, and uhh, idk, Cheese
					}
					break;
			}

		}



		/// <summary>
		/// Put appendage into a Fun Time mode
		/// <param name="_cream">Are we having a good time? Or we done?</param>
		public static void AppendageActionOnRuntime(Character _char, bool _leak = false, bool _cream = false, float _enlarge = -1)
		{
			int index = ActiveAppendages.FindIndex(x => x._Character == _char);     //Honesstly, idk, just copy/pasted from SO, and it works :p
			if (index >= 0)
			{
				ActiveAppendages[index]._Animator.SetBool("FunTime", _cream);     //Set _fun to a speciffic character's appendage
				ActiveAppendages[index]._Animator.SetBool("Leak", _leak);
				if (_enlarge != -1)
					ActiveAppendages[index]._Animator.SetFloat("Enlarge", _enlarge);

				MelonCoroutines.Start(HandleCreamScreen(_cream, _char));   //Start appendage routine for a fresh new appendage
			}
		}



		/// <summary>
		/// Put appendage into a Fun Time mode
		/// <param name="_cream">Are we having a good time? Or we done?</param>
		public static void AppendageActionOnRuntimeEveryone(bool _leak = false, bool _cream = false, float _enlarge = -1)
		{
			foreach (Appendage _appendage in ActiveAppendages)
			{
				_appendage._Animator.SetBool("FunTime", _cream);                  //Set _fun to every character on a level
				_appendage._Animator.SetBool("Leak", _leak);
				if (_enlarge != -1)
					_appendage._Animator.SetFloat("Enlarge", _enlarge);
			}
		}


		/// <summary>
		/// Put appendage into a Fun Time mode
		public void AppendagePositionUpdate(Character _char)
		{
			if (Instance.galleryManager)                                //Uhhh, Yes. Later when i speciffically focus on the animations~
				return;

			else
			{
				int _appendageID = ActiveAppendages.FindIndex(x => x._Character == _char);     //Honesstly, idk, just copy/pasted from SO, and it works :p
				int _characterID;

				_characterID = activeCharactersGameplay.FindIndex(x => x.character == _char);

				if (_appendageID >= 0)
				{
					switch (_char)
					{
						case Character.Sammy:
							break;

						case Character.Nile:
							ActiveAppendages[_appendageID]._Animator.SetBool("NileWindow", (activeCharactersGameplay[_characterID].Animator.GetBool("WindowL") || activeCharactersGameplay[_characterID].Animator.GetBool("WindowR")));
							break;

						case Character.Poppi:
							ActiveAppendages[_appendageID]._Animator.SetBool("PoppiWinR", activeCharactersGameplay[_characterID].Animator.GetBool("WindowR"));     //Set _fun to a speciffic character's appendage
							ActiveAppendages[_appendageID]._Animator.SetBool("PoppiWinL", activeCharactersGameplay[_characterID].Animator.GetBool("WindowL"));     //Set _fun to a speciffic character's appendage
							break;

						case Character.Misty:
							break;

						case Character.Maddie:
							ActiveAppendages[_appendageID]._Animator.SetBool("MaddieDoor", activeCharactersGameplay[_characterID].Animator.GetBool("PeakR"));
							break;

						case Character.Ari:
							break;

						case Character.Kass:
							break;

						case Character.AriBM:
							break;

						case Character.Prinny:
							break;
					}

				}
			}
		}



		private static IEnumerator HandleCreamScreen(bool _cream, Character _character)
		{
			if (!Instance.levelManager) //Check if we're on an actual level
				yield break;                //Stop if not

			if (_cream)
			{
				if (Instance.activeCreamScreen) //Check if there's already a cream screen
					yield break;            //Stop if there is
			}
			else            //Stop creaming
			{
				if (Instance.activeCreamScreen)
					UnityEngine.Object.Destroy(Instance.activeCreamScreen);
				yield break;
			}

			bool _hipFacingCamera = false;      //Chaaracter's hip facing a camera

			switch (LevelManagerBase.Level)     //Characters have different animaations, depending on a level
			{
				case Level.SecurityOffice:
					switch (_character)
					{
						case Character.SammySO:
							_hipFacingCamera = true;
							break;
						case Character.NileSO:
							break;
						case Character.PoppiSO:
							break;
						case Character.MistySO:
							break;
						case Character.MaddieSO:
							break;
						case Character.AriSO:
							break;
						default:
							break;
					}
					break;

				case Level.Nightclub:
					switch (_character)
					{
						case Character.SammyNC:
							_hipFacingCamera = true;
							break;
						case Character.NileNC:
							_hipFacingCamera = true;
							break;
						case Character.PoppiNC:
							break;
						case Character.MistyNC:
							break;
						case Character.MaddieNC:
							break;
						case Character.AriNC:
							break;
						default:
							break;
					}
					break;
			}

			if (_hipFacingCamera)
				Instance.activeCreamScreen = UnityEngine.Object.Instantiate(Instance.prefabCreamScreen, Camera.main.transform);   //Spawm Splooge screen
			yield return null;
		}




		#endregion




		#region UI





		private void SetArcadeCGLoseBackground()
		{
			if (!arcadeBackgroundLoseAppendageLayer)
				return;

			//Originally wanted to do something like "oi oi oi ..arcadeGOchar[ (byte)Character % 1000 ] ;"
			//and use result as a switch for a subtype, but turns out deveopers are inconsistent with it,
			//like, Most of character's SO = charID / *000 and SO = *001, but A is *002, *003 and even *004
			//which is ABSURD and not consistent in any way. So, long long long switch state it is. Bleh.
			//

			switch (attackedCharacter)
			{
				case Character.Sammy:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOSammy[0];
					break;
				case Character.SammySO:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOSammy[0];
					break;
				//case Character.SammyNC:
				//		break;
				case Character.Sammy64:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOSammy[3];
					break;
				case Character.SammyA:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOSammy[2];
					break;
				//	case Character.SammyGoon:
				//		break;
				//	case Character.SammyCheshire:
				//		break;
				case Character.Poppi:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOPoppi[0];
					break;
				case Character.PoppiSO:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOPoppi[0];
					break;
					//	case Character.PoppiNC:
					//		break;
					//	case Character.PoppiA:
					//		break;
					//	case Character.PoppiMadHatter:
					//		break;
					//	case Character.PoppiSummer:
					//		break;
					//		case Character.Ari:
					//			arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOAri[0];
					//			break;
					//		case Character.AriSO:
					//			arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOAri[0];
					//			break;
					//		case Character.AriNC:
					//			arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOAri[0];
					//			break;
					//		case Character.AriBM:
					//			arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOAri[0];
					break;
				case Character.AriA:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOAri[2];
					break;
					//	case Character.AriSummer:
					//			arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOAri[0];
					break;
				case Character.Misty:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOMisty[0];
					break;
				case Character.MistySO:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOMisty[0];
					break;
				//			case Character.MistyNC:
				//
				//				break;
				//			case Character.MistyA:
				//
				//				break;
				//			case Character.MistySummer:
				//
				//				break;
				//			case Character.Maddie:
				//				break;
				//			case Character.MaddieSO:
				//				break;
				case Character.MaddieNC:
					arcadeBackgroundLoseAppendageLayer.sprite = arcadeGOMaddie[1];
					break;
				//		case Character.MaddieA:
				//			break;
				//		case Character.MaddieSummer:
				//			break;
				//		case Character.Kass:
				//			break;
				//		case Character.KassSO:
				//			break;
				//		case Character.KassNC:
				//			break;
				//		case Character.Prinny:
				//			break;
				//		case Character.Heisenbones:
				//			break;
				//		case Character.HeisenbonesSO:
				//			break;
				//		case Character.HeisenbonesCaptain:
				//			break;
				default:
					arcadeBackgroundLoseAppendageLayer.gameObject.SetActive(false);
					return;
			}

			arcadeBackgroundLoseAppendageLayer.enabled = true;
			arcadeBackgroundLoseAppendageLayer.gameObject.SetActive(true);
		}




		/// <summary>
		/// Creates a generic toggle in the Adult section of Settings.
		/// </summary>
		/// <param name="_title">Title teхt for toggle in settings</param>
		/// <param name="_void">Toggle action</param>
		/// <param name="_initialState">Set toggles' initial state on spawn</param>
		public static void InstantiateToggleInSettings(string _title, Action<bool> _void, bool _initialState)
		{
			GameObject _toggle = UnityEngine.Object.Instantiate(GameObject.Find("Radio - Adult Content"), Instance.adultSection);    //Get and spawn a generic toggle

			UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontEvent>());             //Remove translation junk
			UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontMaterialEvent>());     //Remove translation junk
			UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeStringEvent>());              //Remove translation junk

			_toggle.GetComponentInChildren<TextMeshProUGUI>().text = _title;                    //Set toggle title teхt
			_toggle.GetComponent<UIRadio>().SetRadio(_initialState, false);                                    //Set initial state for toggle
			_toggle.GetComponent<UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(_void));  //Assign listener to a state change
		}



		/// <summary>
		/// Spawns: Intersex Mode and Topless Mode toggles into settings menu.
		/// </summary>
		private void InstantiateCustomSettingItems()
		{
			if (adultSection)       //Check if container is assigned
				return;

			GameObject _radio = GameObject.Find("Radio - Adult Content");
			_radio.GetComponent<UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(AdultToggleListener));  //Assign listener to a state change
			adultSection = _radio.transform.parent.GetComponent<RectTransform>();       //Get and assign a container in Settings that stores all the Adult stuff

			InstantiateToggleInSettings("Intersex Mode", IntersexToggleListener, (PlayerPrefs.GetString("Intersex") == "True"));  //Spawn Interseх toggle
			InstantiateToggleInSettings("Topless Mode", ToplessToggleListener, (PlayerPrefs.GetString("Topless") == "True"));     //Spawn Topless toggle
		}



		/// <summary>
		///	Spawns: *Character* Cream button on an act screen.
		/// </summary>
		private void InstantiateCharacterCreamButton(ScenePlayerUIManager _instance)
		{
			if (creamButton || !Intersex)      //Check if Creaming button already eхists or Interseх mode is off
				return;

			GameObject genericButton = _instance.intensityButton.gameObject;

			creamButton = GameObject.Instantiate(genericButton.GetComponent<Button>(), genericButton.transform);    //Get and assign a Settings button from the menu
			creamButton.GetComponent<RectTransform>().localPosition += new Vector3(-225, 0, 0);                     //Move a new button to the left
			creamButton.gameObject.name = "Button - Character Cream";

			UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeTMPFontEvent>());                 //Remove translation junk
			UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeTMPFontMaterialEvent>());         //Remove translation jonk       !TODO/!HELP: I HAVE NO IDEA IF IT'S POSSIBLE TO ADD
			UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeStringEvent>());                  //Remove translation junk        A CUSTOM TRANSLATION STUFF INTO TRANSLATION TABLE
			UnityEngine.Object.Destroy(creamButton.transform.GetChild(2).gameObject);                               //Remove translation junk        

			creamButton.onClick.AddListener(new Action(() => { CharacterCreamToggle(); }));                         //Add listener to button presses

			if (!galleryManager)                                                                                        //Buttons are available from get go in Gallery
				creamButton.interactable = false;
		}



		private void AdultToggleListener(bool _isOn)
		{
			Adult = _isOn;

			ApplyAdultToggles();
		}



		private void IntersexToggleListener(bool _isOn)
		{
			PlayerPrefs.SetString("Intersex", _isOn.ToString());            //Save state of toggle

			ApplyAdultToggles();
		}



		private void ToplessToggleListener(bool _isOn)
		{
			PlayerPrefs.SetString("Topless", _isOn.ToString());             //Save state of toggle

			ApplyAdultToggles();
		}



		/// <summary>
		/// Toggle Character Creaming
		/// </summary>
		public void CharacterCreamToggle()
		{
			creaming = !creaming;
			AppendageActionOnRuntime(attackedCharacter, false, creaming);

			if (creamButton)
				creamButton.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = creaming ? 1 : 0;
		}



		/// <summary>
		/// Force Character into Creaming
		/// </summary>
		public void CharacterCreamForce(bool _creaming)
		{
			creaming = _creaming;
			AppendageActionOnRuntime(attackedCharacter, false, creaming);

			if (creamButton)
				creamButton.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = creaming ? 1 : 0;
		}



		public void CharacterGalleryApplyFutaSettings(float _exc)
		{
			AppendageActionOnRuntimeEveryone(false, leakToggleGallery.isOn, _exc);
		}



		public void CharacterGalleryApplyFutaSettings(bool _leak)
		{
			AppendageActionOnRuntimeEveryone(false, leakToggleGallery.isOn, excitementSliderGallery.value);
		}



		private void ApplyAdultToggles()
		{
			Intersex = (PlayerPrefs.GetString("Intersex") == "True") && Adult;      //Check if Intersex and H-Content toggles are active
			Topless = (PlayerPrefs.GetString("Topless") == "True") && Adult;        //Check if Topless and H-Content toggles are active

			foreach (Appendage _appendage in ActiveAppendages)
				_appendage._Animator.SetBool("Topless", Topless);

			foreach (Appendage _appendage in ActiveAppendages)
				_appendage._Animator.SetBool("Intersex", Intersex);


			if (LevelManagerBase.Exists)
			{
				if (Topless)
					foreach (CharacterControllerBase item in activeCharactersGameplay)
						item.CostumeSwitcher.SwitchVariant("Nude");//Beach Party!
				else
				{
					foreach (CharacterControllerBase item in activeCharactersGameplay)
						item.CostumeSwitcher.SwitchVariant(item.CostumeSwitcher.defaultVariant);//Everyone, put the clothes on!
				}
				if (Instance.creamButton)
					creamButton.gameObject.SetActive(Intersex);
			}                                                       //DOESN'T APPLY RIGHT AWAT FOR SOME REASON
		}




		#endregion




		#region Patching




		#region Characters




		/// <summary>
		/// Called when character is ready
		/// </summary>
		[HarmonyPatch(typeof(CharacterControllerBase), "Initialize")]
		private static class CharacterGameplayInit
		{
			public static void Postfix(ref CharacterControllerBase __instance)            //Original script patch + character _instance that had called it
			{

				if (Topless && __instance.CostumeSwitcher)
					__instance.CostumeSwitcher.SwitchVariant("Nude");                     //Undress character

				activeCharactersGameplay.Add(__instance);                                 //Add this character to active characters

				if (!__instance.Animator)
					return;

				Transform[] _armatureBones = __instance.Animator.transform.GetComponentsInChildren<Transform>();    //Get all the bones in the armature

				foreach (Transform _bone in _armatureBones)
				{
					if (_bone.name == "DEF-spine")                                        //Not effective, but just in case if character's hierarchy gets messed up. Animator.Avatar / .GetBoneTra returns null 
					{

						Instance.InstantiateAppendage(__instance.Character, __instance.Animator, _bone.transform);  //Instantiate appendage in Hip bone
						break;
					}
				}
			}
		}



		/// <summary> 
		/// ...i honestly no idea on how to do it properly, tried charactermanager, gallerycontroller, and many other things.
		/// simply, can't get it work in any other way. It will stay like that for now, until i gt enough motivation to figure
		/// things out.
		/// </summary>
		/// d
		[HarmonyPatch(typeof(GalleryCharacterButton), "Initialize", new Type[] { typeof(int), typeof(GalleryCharacter) })]
		private static class GalleryManagerssss
		{
			public static void Postfix(ref GalleryCharacterButton __instance, int buttonIndex, GalleryCharacter character)
			{

				if (!activeCharactersGallery.Contains(character))
				{
					activeCharactersGallery.Add(character);                               //Add this character to active characters

					Transform[] _armatureBones = character.Animator.transform.GetComponentsInChildren<Transform>();    //Get all the bones in the armature
					foreach (Transform _bone in _armatureBones)
					{
						if (_bone.name == "DEF-spine")                                      //Not effective, but just in case if character's hierarchy gets messed up. Animator.Avatar / .GetBoneTra returns null 
						{
							Instance.InstantiateAppendage(character.character, character.Animator, _bone.transform);  //Instantiate appendage in Hip bone
							break;
						}
					}

					if (Topless)
						character._outfitSwitcher.SwitchVariant("Nude");                     //Undress character
				}

				Character _char = character.character;

				int _appendageID = 999;
				_appendageID = ActiveAppendages.FindIndex(x => x._Character == _char);     //Honesstly, idk, just copy/pasted from SO, and it works

				if (_appendageID == -1)
					return;

				ActiveAppendages[_appendageID]._GameObject.SetActive(Intersex && character._outfitSwitcher.CurrentVariant == "Nude");
				ActiveAppendages[_appendageID]._Animator.SetFloat("Enlarge", Instance.excitementSliderGallery.value);
				ActiveAppendages[_appendageID]._Animator.SetBool("FunTime", Instance.leakToggleGallery.isOn);

			}
		}
		/// <summary>
		/// Called when character's outfit switched in Gallery
		/// </summary>
		[HarmonyPatch(typeof(GalleryCharacter), "SwitchOutfit")]
		private static class CharacterGalleryOutfitSwitch
		{
			public static void Postfix(ref GalleryCharacter __instance)
			{
				Character _char = __instance.character;

				int _appendageID = 999;
				_appendageID = ActiveAppendages.FindIndex(x => x._Character == _char);     //Honesstly, idk, just copy/pasted from SO, and it works

				if (_appendageID == -1)
					return;

				ActiveAppendages[_appendageID]._GameObject.SetActive(Intersex && __instance._outfitSwitcher.CurrentVariant == "Nude");
				ActiveAppendages[_appendageID]._Animator.SetFloat("Enlarge", Instance.excitementSliderGallery.value);
				ActiveAppendages[_appendageID]._Animator.SetBool("FunTime", Instance.leakToggleGallery.isOn);

			}
		}



		/// <summary>
		/// Called when character forced into playing Gallery animation
		/// </summary>
		[HarmonyPatch(typeof(GalleryCharacter), "TriggerAnimation")]
		private static class CharacterGalleryAnimate
		{
			public static void Postfix(ref GalleryCharacter __instance)
			{
				AppendageActionOnRuntime(__instance.Character, false, Instance.leakToggleGallery.isOn, Instance.excitementSliderGallery.value);
				Instance.AppendagePositionUpdate(__instance.character);
			}
		}



		/// <summary>
		/// Called after character leans from the door, after FADE IN FROM black screen
		/// </summary>
		[HarmonyPatch(typeof(NightclubCharacterController), "OnAttackTransitionEnd")]
		private static class ClubActStart                                           //Function nickname for myself, so i won't forget what it's for
		{
			public static void Prefix(ref NightclubCharacterController __instance)  //Call stuff before original script's events accure
			=> AppendageActionOnRuntime(__instance.character, false, true);         //Start creaming
		}



		/// <summary>
		/// Being called after the fade in after the Attack Scene had ended
		/// </summary>
		[HarmonyPatch(typeof(NightclubCharacterController), "BeforeTimeSkip")]
		private static class ClubActEnd
		{
			public static void Postfix(ref NightclubCharacterController __instance)  //Call stuff before original script's events accure
			{
				if (Topless)                                                         //Check if Topless toggle is on
					__instance.CostumeSwitcher.SwitchVariant("Nude");                //Undress the character

				AppendageActionOnRuntime(__instance.character, false, false);        //Stop creaming
			}
		}



		/// <summary>
		/// Sitches character to Beach Mode after having a Fun time with a customer, sice they attempt to put it back on
		/// </summary>
		[HarmonyPatch(typeof(NightclubCharacterController), "MoveToRandom")]
		private static class ClubCustomerActEnd
		{
			public static void Postfix(ref NightclubCharacterController __instance)
			{
				if (Topless)                                                        //Check if Topless toggle is on
					__instance.CostumeSwitcher.SwitchVariant("Nude");               //Undress the character
			}
		}



		/// <summary>
		/// Being called every time character is moved to Speciffic point on the map, NOT random roaming
		/// </summary>
		[HarmonyPatch(typeof(SecurityOfficeCharacterController), "MoveTo")]
		private static class CharacterOfficeMoved
		{
			public static void Postfix(ref SecurityOfficeCharacterController __instance)
			{
				Instance.AppendagePositionUpdate(__instance.character);
			}
		}



		/// <summary>
		/// 
		/// </summary>
		[HarmonyPatch(typeof(SecurityOfficeCharacterController), "AttackPlayer", new Type[] { typeof(bool), typeof(string) })]
		private static class CharacterOfficeAttack
		{
			public static void Prefix(ref SecurityOfficeCharacterController __instance, bool overrideAnimationTrigger, string overrideTrigger)
			{
				//	if (__instance.character == Characters.Nile)
				//	__instance.Animator.runtimeAnimatorController = Instance.animPlace;

				attackedCharacter = __instance.character;
				AppendageActionOnRuntime(__instance.character, false, false, 1);

				if (Instance.creamButton)
					Instance.creamButton.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = attackedCharacter + "\nCum";
			}
		}




		/// <summary>
		/// 
		/// </summary>
		[HarmonyPatch(typeof(ArcadeCharacterController), "AttackPlayer", new Type[] { typeof(bool), typeof(string) })]
		private static class CharacterArcadeAttack
		{
			public static void Prefix(ref ArcadeCharacterController __instance, bool overrideAnimationTrigger, string overrideTrigger)
				=> attackedCharacter = __instance.character;
		}




		#endregion




		#region UI




		void EnableCreamButton()
		{
			if (creamButton)
				creamButton.interactable = true;
		}



		/// <summary>
		/// Settings Menu opens for a first time
		/// </summary>
		[HarmonyPatch(typeof(SettingsUIManager), "Awake")]
		private static class SettingsMenuOpen
		{
			public static void Postfix(ref SettingsUIManager __instance)
			=> Instance.InstantiateCustomSettingItems();
		}



		/// <summary>
		/// Game Over screen in Office
		/// </summary>
		[HarmonyPatch(typeof(GameResultLosePanel), "ShowPanel")]
		private static class GameOverScreenShowed
		{
			public static void Postfix(ref GameResultLosePanel __instance)
			=> Instance.CharacterCreamForce(false);
		}



		/// <summary>
		/// Game Over screen in Office
		/// </summary>
		[HarmonyPatch(typeof(ArcadeCGDisplay), "OnCGLoaded")]
		private static class GameOverScreenShowdded
		{
			public static void Postfix(ref ArcadeCGDisplay __instance)
			{
				if (!Intersex || !Adult)
					return;


				if (!Instance.arcadeBackgroundLoseAppendageLayer)
				{
					GameObject _lyr = __instance.cgImage.gameObject;
					Instance.arcadeBackgroundLoseAppendageLayer = GameObject.Instantiate(_lyr.GetComponent<Image>(), __instance.cgImage.transform.parent);
					Instance.arcadeBackgroundLoseAppendageLayer.transform.SetSiblingIndex(__instance.cgImage.transform.GetSiblingIndex() + 1);
					Instance.arcadeBackgroundLoseAppendageLayer.raycastTarget = false;
				}

				Instance.SetArcadeCGLoseBackground();
			}
		}





		/// <summary>
		/// Game Over screen in Office
		/// </summary>
		[HarmonyPatch(typeof(AdultScenePlayer), "OnIntroEnd")]
		private static class OnAttackOver
		{
			public static void Postfix(ref AdultScenePlayer __instance)
			=> Instance.EnableCreamButton();
		}



		/// <summary>
		/// Intencity Pre-Game Over screen in office
		/// </summary>
		[HarmonyPatch(typeof(ScenePlayerUIManager), "Awake")]
		private static class ActScreenShowed
		{
			public static void Prefix(ref ScenePlayerUIManager __instance)
			=> Instance.InstantiateCharacterCreamButton(__instance);   //Start appendage routine for a fresh new appendage
		}











		#endregion




		#endregion




		#region Online Update Notiff



		static void FetchLatestVersionFromGithub()
		{
			///<summary>
			///	startcoroutine(www.fetch(foxcomment/inheatfutanari/latest), out string _ver)
			/// if(_ver != melonmod.version)
			///		UpdateAvailable(_ver);
			/// 
			/// 
			/// void UpdateAvailable(string _latestVer) =>
			///		text1.text = "Newer version of mod is available:\n" + _latestVer; 
			/// </summary>
		}




		#endregion




#if !RELEASE
		#region DEBUG
		#endregion
#endif
	}

}
