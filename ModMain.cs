using MelonLoader;
using HarmonyLib;
using UnityEngine;
using FutanariMod;

using MelonLoader.Utils;
using System.Reflection;
using System.Collections;
using Il2CppTMPro;
using Il2CppInterop.Runtime;
using Il2CppMonsterBox.Runtime.Extensions._Localization;
using Il2CppMonsterBox.Runtime.Core.Characters.Enums;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Character;
using Il2CppMonsterBox.Runtime.Gameplay.SecurityOffice;
using Il2CppMonsterBox.Runtime.Gameplay.Scene.Adult;
using Il2CppMonsterBox.Runtime.Gameplay.Character;
using Il2CppMonsterBox.Runtime.Gameplay.Level;
using Il2CppMonsterBox.Runtime.Gameplay.Enums;
using Il2CppMonsterBox.Runtime.UI.Game.Result;
using Il2CppMonsterBox.Runtime.UI.Game.Adult;
using Il2CppMonsterBox.Systems.UI.Elements;
using Il2CppMonsterBox.Runtime.UI.Settings;
using Il2CppMonsterBox.Systems.Saving;
using Il2CppMonsterBox.Runtime.Gallery;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using UnityEngine.UI;
using Il2CppMonsterBox.Runtime.UI.Gallery.Character;

//using UnityEngine.Animations;
//using Il2CppMonsterBox.Systems.Tools.Miscellaneous.Gameplay;

[assembly: MelonInfo(typeof(ModMain), "Futanari Mod", "2025.5.8", "FoxComment", "https://github.com/foxcomment/IN-HEAT_Futanari-Mod")]
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
			public Characters _Character;
			public Animator _Animator;
			public Appendage(GameObject _gameobject, Characters _character, Animator _animator)
			{
				_GameObject = _gameobject;
				_Character = _character;
				_Animator = _animator;
			}
		}

		private const string addressField = "FutanariMod.funny";
		private const string lazyAddressField = "FoxCom-Futanari-AB";

		private const string appendageSammyPrefabFile = "Assets/Futa/Prefs/AppendageSammy.prefab";
		private const string appendageMaddiePrefabFile = "Assets/Futa/Prefs/AppendageMaddie.prefab";
		private const string appendagePoppiPrefabFile = "Assets/Futa/Prefs/AppendagePoppi.prefab";
		private const string appendageNilePrefabFile = "Assets/Futa/Prefs/AppendageNile.prefab";
		private const string appendageAriPrefabFile = "Assets/Futa/Prefs/AppendageAri.prefab";
		private const string appendageMistyPrefabFile = "Assets/Futa/Prefs/AppendageMisty.prefab";

		private const string creamScreenPrefabFile = "Assets/Futa/Prefs/CreamScreen.prefab";
		private const string futaGalleryOptionsPrefabFile = "Assets/Futa/Prefs/Futa Gallery.prefab";
		private const string modLogoPrefabFile = "Assets/Futa/Prefs/ModLog.prefab";
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

		Shader defaultShader;

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
		/// Not sure where to obtain info on an attacking character properly tbh, so heres the workaround
		/// </summary>
		static Characters attackedCharacter;

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



		void LoadAssetBundle()
		{
			if (!CheckAssetBundlePresence(true))
				return;

			Assembly assembly = MelonAssembly.Assembly;
			RequestAB = AssetBundle.LoadFromFileAsync(MelonEnvironment.ModsDirectory + "\\" + lazyAddressField);
			Bundle = RequestAB.assetBundle;

			MelonLogger.Warning("Please, make sure that you're using the right version of '" + lazyAddressField +
			"' from the same download as the .dll file itself.\nOtherwise you Will get a bunch of errors and a potential game crash.");

			prefabAppendageSammy = UnpackAssetGameObject(appendageSammyPrefabFile);
			prefabAppendageSammy.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageAri = UnpackAssetGameObject(appendageAriPrefabFile);
			prefabAppendageAri.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageMisty = UnpackAssetGameObject(appendageMistyPrefabFile);
			prefabAppendageMisty.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendagePoppi = UnpackAssetGameObject(appendagePoppiPrefabFile);
			prefabAppendagePoppi.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageNile = UnpackAssetGameObject(appendageNilePrefabFile);
			prefabAppendageNile.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabAppendageMaddie = UnpackAssetGameObject(appendageMaddiePrefabFile);
			prefabAppendageMaddie.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabCreamScreen = UnpackAssetGameObject(creamScreenPrefabFile);
			prefabCreamScreen.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabFutaGallerySettings = UnpackAssetGameObject(futaGalleryOptionsPrefabFile);
			prefabFutaGallerySettings.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			prefabModLogo = UnpackAssetGameObject(modLogoPrefabFile);
			prefabModLogo.hideFlags |= HideFlags.DontUnloadUnusedAsset;

			//animTEST = UnpackAssetAnimation(animTestFile);

			MelonLogger.Msg(lazyAddressField + " is loaded.");

			//animTEST = UnpackAssetAnimator(animTestFile);
			//animTEST.hideFlags |= HideFlags.DontUnloadUnusedAsset;
		}



		void InstantiateModLogo()
		{
			modLogo = UnityEngine.Object.Instantiate(prefabModLogo);
			modLogo.transform.localScale = Vector3.one * .1f;
			modLogo.transform.position = new Vector3(-.029f, .49f, .45f);
			modLogo.transform.rotation = Quaternion.Euler(0, 193.435f, 0.02f);
		}



		void FetchDefaultAssets()
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



		void CheckSettingsPresence()
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

				MelonLogger.Msg("GALLERY");//Add listener to button presses
			}

			CheckSettingsPresence();
			FetchDefaultAssets();

			if (sceneName.Contains("Title"))
				InstantiateModLogo();
		}




		#endregion




		#region Appendage




		public void InstantiateAppendage(Characters _character, Animator _c_anim, Transform _hipBone)
		{
			GameObject _appendage;

			switch (_character)   //Instntiate a speciffic appendage model
			{
				case Characters.Ari:    //Canine
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageAri, _hipBone);
					break;

				case Characters.Misty:  //Tapered
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageMisty, _hipBone);
					break;

				case Characters.Sammy:  //Humanoid
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageSammy, _hipBone);
					break;

				case Characters.Poppi:  //Humanoid
					_appendage = UnityEngine.Object.Instantiate(prefabAppendagePoppi, _hipBone);
					break;

				case Characters.Maddie:  //Thick
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageMaddie, _hipBone);
					break;

				case Characters.Nile:  //Humanoid
					_appendage = UnityEngine.Object.Instantiate(prefabAppendageNile, _hipBone);
					break;

				default:    //Generic
							//Better to leave it empty, because it had created an appendage on a dor for whatever reason~
					return;
			}

			MelonLogger.Msg("bone: " + _hipBone);
			SkinnedMeshRenderer _mesh = _appendage.transform.GetComponentInChildren<SkinnedMeshRenderer>();
			Animator _a_anim = _appendage.GetComponent<Animator>();

			_mesh.material.shader = defaultShader;                  //Assign default params

			switch (_character)
			{
				case Characters.Ari:
					SetupAppendage(_a_anim, .103f, .49f, 1, 1);
					break;  //Green

				case Characters.Sammy:
					SetupAppendage(_a_anim, .065f, .68f, .3f, .16f);
					break;  //Tiger

				case Characters.Poppi:
					SetupAppendage(_a_anim, .075f, .5f, 0, .1f);
					break;  //Buni

				case Characters.Nile:
					//animPlace = _c_anim.runtimeAnimatorController;
					//_c_anim.runtimeAnimatorController = animTEST;
					SetupAppendage(_a_anim, .091f, .55f, 0, .45f);
					break;  //Black

				case Characters.Maddie:
					SetupAppendage(_a_anim, .2f, .65f, .7f, .5f);
					break;  //Burger

				case Characters.Misty:
					SetupAppendage(_a_anim, .1f, .5f, 1);
					break;  //Shark

				case Characters.Kass:
					UnityEngine.Object.Destroy(_appendage);
					return; //Kass is not an actual enemy character (racoon dumpster thing)

				case Characters.AriBM:
					UnityEngine.Object.Destroy(_appendage);
					return;
			}

			MelonCoroutines.Start(AppendageLifeCycle(_character, _c_anim, _mesh));   //Start appendage routine for a fresh new appendage
		}



		/// <summary>
		/// Set Placement, Scale and overall look.
		/// </summary>
		/// <param name="_anim">Animator</param>
		/// <param name="_offest">How much the appendage should be moved forward (locally)</param>
		/// <param name="_scale">Scale of appendage's GameObject</param>
		/// <param name="_fertility">Determens the size of Two Male Bump. Removes them completely if 0</param>
		/// <param name="_diameter">Additional diameter of an appendage (stacks with _scale)</param>
		void SetupAppendage(Animator _anim, float _offest = .068f, float _scale = .65f, float _diameter = .3f, float _fertility = 0)
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
		private IEnumerator AppendageLifeCycle(Characters _character, Animator _c_anim, SkinnedMeshRenderer _a_mesh)
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
		public static void AppendageActionOnRuntime(Characters _char, bool _leak = false, bool _cream = false, float _enlarge = -1)
		{
			if (_char == Characters.Everyone)
				foreach (Appendage _appendage in ActiveAppendages)
				{
					_appendage._Animator.SetBool("FunTime", _cream);                  //Set _fun to every character on a level
					_appendage._Animator.SetBool("Leak", _leak);
					if (_enlarge != -1)
						_appendage._Animator.SetFloat("Enlarge", _enlarge);
				}
			else
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
		}



		/// <summary>
		/// Put appendage into a Fun Time mode
		public void AppendagePositionUpdate(Characters _char)
		{
			if (Instance.galleryManager)                                //Uhhh, Yes. Later when i speciffically focus on the animations~
				return;

			if (_char == Characters.Everyone)                           //Unoptimised, but i can't be bothered yet~
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
						case Characters.Sammy:
							break;

						case Characters.Nile:
							ActiveAppendages[_appendageID]._Animator.SetBool("NileWindow", (activeCharactersGameplay[_characterID].Animator.GetBool("WindowL") || activeCharactersGameplay[_characterID].Animator.GetBool("WindowR")));
							break;

						case Characters.Poppi:
							ActiveAppendages[_appendageID]._Animator.SetBool("PoppiWinR", activeCharactersGameplay[_characterID].Animator.GetBool("WindowR"));     //Set _fun to a speciffic character's appendage
							ActiveAppendages[_appendageID]._Animator.SetBool("PoppiWinL", activeCharactersGameplay[_characterID].Animator.GetBool("WindowL"));     //Set _fun to a speciffic character's appendage
							break;

						case Characters.Misty:
							break;

						case Characters.Maddie:
							ActiveAppendages[_appendageID]._Animator.SetBool("MaddieDoor", activeCharactersGameplay[_characterID].Animator.GetBool("PeakR"));
							break;

						case Characters.Ari:
							break;

						case Characters.Kass:
							break;

						case Characters.AriBM:
							break;

						case Characters.Arin:
							break;

						case Characters.Saya:
							break;

						case Characters.Prinny:
							break;
					}

				}
			}
		}



		private static IEnumerator HandleCreamScreen(bool _cream, Characters _character)
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
						case Characters.Sammy:
							_hipFacingCamera = true;
							break;
						case Characters.Nile:
							break;
						case Characters.Poppi:
							break;
						case Characters.Misty:
							break;
						case Characters.Maddie:
							break;
						case Characters.Ari:
							break;
						default:
							break;
					}
					break;

				case Level.Nightclub:
					switch (_character)
					{
						case Characters.Sammy:
							_hipFacingCamera = true;
							break;
						case Characters.Nile:
							_hipFacingCamera = true;
							break;
						case Characters.Poppi:
							break;
						case Characters.Misty:
							break;
						case Characters.Maddie:
							break;
						case Characters.Ari:
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




		/// <summary>
		/// Creates a generic toggle in the Adult section of Settings.
		/// </summary>
		/// <param name="_title">Title teхt for toggle in settings</param>
		/// <param name="_void">Toggle action</param>
		/// <param name="_initialState">Set toggles' initial state on spawn</param>
		public static void InstantiateToggleInSettings(string _title, Action<bool> _void, bool _initialState)
		{
			GameObject _toggle = UnityEngine.Object.Instantiate(GameObject.Find("Radio - H Content"), Instance.adultSection);    //Get and spawn a generic toggle

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
		void InstantiateCustomSettingItems()
		{
			if (adultSection)       //Check if container is assigned
				return;

			GameObject _radio = GameObject.Find("Radio - H Content");                   //Find a toogle in Adult section and use it as a base for mine
			_radio.GetComponent<UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(AdultToggleListener));  //Assign listener to a state change
			adultSection = _radio.transform.parent.GetComponent<RectTransform>();       //Get and assign a container in Settings that stores all the Adult stuff

			InstantiateToggleInSettings("Intersex Mode", IntersexToggleListener, (PlayerPrefs.GetString("Intersex") == "True"));  //Spawn Interseх toggle
			InstantiateToggleInSettings("Topless Mode", ToplessToggleListener, (PlayerPrefs.GetString("Topless") == "True"));     //Spawn Topless toggle
		}



		/// <summary>
		///	Spawns: *Character* Cream button on an act screen.
		/// </summary>
		void InstantiateCharacterCreamButton(ScenePlayerUIManager _instance)
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



		void AdultToggleListener(bool _isOn)
		{
			Adult = _isOn;

			ApplyAdultToggles();
		}



		void IntersexToggleListener(bool _isOn)
		{
			PlayerPrefs.SetString("Intersex", _isOn.ToString());            //Save state of toggle

			ApplyAdultToggles();
		}



		void ToplessToggleListener(bool _isOn)
		{
			PlayerPrefs.SetString("Topless", _isOn.ToString());             //Save state of toggle

			ApplyAdultToggles();
		}



		/// <summary>
		/// Toggle Character Creaming
		/// </summary>
		void CharacterCreamToggle()
		{
			creaming = !creaming;
			AppendageActionOnRuntime(attackedCharacter, false, creaming);

			if (creamButton)
				creamButton.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = creaming ? 1 : 0;
		}



		/// <summary>
		/// Force Character into Creaming
		/// </summary>
		void CharacterCreamForce(bool _creaming)
		{
			creaming = _creaming;
			AppendageActionOnRuntime(attackedCharacter, false, creaming);

			if (creamButton)
				creamButton.transform.GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = creaming ? 1 : 0;
		}



		void CharacterGalleryApplyFutaSettings(float _exc)
		{
			AppendageActionOnRuntime(Characters.Everyone, false, false, _exc);
		}



		void CharacterGalleryApplyFutaSettings(bool _leak)
		{
			AppendageActionOnRuntime(Characters.Everyone, false, leakToggleGallery.isOn, excitementSliderGallery.value);
		}



		void ApplyAdultToggles()
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
						item.CostumeSwitcher.SwitchVariant("Nude");                             //Beach Party!
				else
					foreach (CharacterControllerBase item in activeCharactersGameplay)
						item.CostumeSwitcher.SwitchVariant(item.CostumeSwitcher.defaultVariant);//Everyone, put the clothes on!

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
				MelonLogger.Msg(" CHASR INITEDDD GAMEPS");
				if (Topless)
					__instance.CostumeSwitcher.SwitchVariant("Nude");                     //Undress character

				activeCharactersGameplay.Add(__instance);                                 //Add this character to active characters

				Transform[] _armatureBones = __instance.Animator.transform.GetComponentsInChildren<Transform>();    //Get all the bones in the armature
				foreach (Transform _bone in _armatureBones)
				{
					if (_bone.name == "DEF-spine")                                        //Not effective, but just in case if character's hierarchy gets messed up. Animator.Avatar / .GetBoneTra returns null 
					{

						MelonLogger.Msg("Appendage INIT:." + __instance.character);
						Instance.InstantiateAppendage(__instance.character, __instance.Animator, _bone.transform);  //Instantiate appendage in Hip bone
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
		/// 
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

				Characters _char = character.character;

				int _appendageID = 999;
				_appendageID = ActiveAppendages.FindIndex(x => x._Character == _char);     //Honesstly, idk, just copy/pasted from SO, and it works

				MelonLogger.Msg("Active: " + character.character + "  " + _appendageID);
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
				Characters _char = __instance.character;

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
		[HarmonyPatch(typeof(SecurityOfficeCharacterController), "AttackPlayer")]
		private static class CharacterOfficeAttack
		{
			public static void Prefix(ref SecurityOfficeCharacterController __instance)
			{
				//	if (__instance.character == Characters.Nile)
				//	__instance.Animator.runtimeAnimatorController = Instance.animPlace;

				attackedCharacter = __instance.character;
				AppendageActionOnRuntime(__instance.character, false, false, 1);

				if (Instance.creamButton)
					Instance.creamButton.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = attackedCharacter + "\nCum";
			}
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



}
