using MelonLoader;
using UnityEngine;
using FutanariMod;
using Il2CppMonsterBox;
using System.Collections;
using UnityEngine.UI;
using Il2CppInterop.Runtime;
using Il2CppMonsterBox.Runtime.Core.Gameplay;
using Il2CppMonsterBox.Systems.Tools.Miscellaneous;
using Il2CppMonsterBox.Systems;
using Il2CppMonsterBox.Systems.Tools;
using Il2CppMonsterBox.Systems.Tools.Accessors;
using Il2CppMonsterBox.Systems.Tools.Interfaces;
using Il2CppMonsterBox.Systems.Tools.PubSub;
using Il2CppMonsterBox.Systems.Tools.Helpers;
using Il2CppMonsterBox.Systems.Tools.PubSub.Interfaces;
using Il2CppMonsterBox.Systems.Camera;
using Il2CppMonsterBox.Systems.Camera.Scriptable;
using Il2CppMonsterBox.Systems.Camera.Enums;
using Il2CppMonsterBox.Systems.Camera.DTO;
using Il2CppMonsterBox.Systems.Input;
using Il2CppMonsterBox.Systems.SharpConfig;
using Il2CppMonsterBox.Systems.Saving.Enums;
using Il2CppMonsterBox.Systems.Saving.Scriptables;
using Il2CppMonsterBox.Systems.Saving.Data;
using Il2CppMonsterBox.Systems.Confirmation.Adult;
using Il2CppMonsterBox.Systems.Dialogue.UI.Keyboard;
using Il2CppMonsterBox.Systems.Dialogue.UI;
using Il2CppMonsterBox.Runtime.Core.Initializers;
using Il2CppMonsterBox.Runtime.Core.SceneManagement;
using Il2CppMonsterBox.Runtime.Core.Scriptable;
using Il2CppMonsterBox.Runtime.Extensions._Localization;
using Il2CppMonsterBox.Extensions.Addressables;
using Il2CppMonsterBox.Extensions.Rewired.Button_Hint.UI;
using Il2CppMonsterBox.Extensions.Rewired.Button_Hint;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Character;
using Il2CppMonsterBox.Runtime.Gameplay;
using Il2CppMonsterBox.Systems.Tools.Miscellaneous.Adult;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Enums;
using Il2CppMonsterBox.Runtime.UI.Settings;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Level;
using Il2CppMonsterBox.Runtime.Gameplay.Enums;
using Il2CppMonsterBox.Runtime.Gameplay.SecurityOffice;
using Il2CppMonsterBox.Runtime.Gameplay.Character;
using Il2CppMonsterBox.Runtime.Gameplay.Interactables;
using Il2CppMonsterBox.Systems.UI.Elements;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.Startup;
using Il2CppInterop.Runtime.Runtime;
using Il2CppInterop.Common.Attributes;
using HarmonyLib;
using Il2CppMonsterBox.Runtime.Gameplay.Level;
using Il2CppMonsterBox.Runtime.Gameplay.SecurityOffice.Enums;
using Il2CppTMPro;
using Il2CppMonsterBox.Runtime.UI.Game.Pause;
using Il2CppMonsterBox.Runtime.UI.Game.Result;
using Mono.CSharp;
using UnityEngine.Rendering.Universal.LibTessDotNet;
using UnityEngine.TextCore.Text;
using Il2Cpp;
using Il2CppMonsterBox.Systems.Saving;
using MelonLoader.TinyJSON;

[assembly: MelonInfo(typeof(ModMain), "Futanari Mod", "2024.9.30", "FoxComment", "https://github.com/foxcomment/IN-HEAT_Futanari-Mod")]
[assembly: MelonGame("MonsterBox", "IN HEAT")]
namespace FutanariMod
{
    //ADD CREAMING TOGGLE ON GAME OVER SCREEN (MAKES CHARACTER COOM)
    public class ModMain : MelonMod
    {
        public static List<Appendage> activeAppendages = new List<Appendage>(0);
        public struct Appendage
        {
            public GameObject _GameObject;
            public Characters _Character;
            public Animator _Animator;

            public Appendage(GameObject _gameobject, Characters _character, Animator _animator) //To make   new App(S,Y,A);
            {
                _GameObject = _gameobject;
                _Character = _character;
                _Animator = _animator;
            }
        }

        private const string addressField = "InHeatFutaMod.funny";

        private const string textureSammyFile = "Assets/Futa/Textures/SammyV1.png";
        private const string textureMaddieFile = "Assets/Futa/Textures/MaddyV1.png";
        private const string texturePoppiFile = "Assets/Futa/Textures/PoppiV1.png";
        private const string textureAriFile = "Assets/Futa/Textures/AriV2.png";
        private const string textureNileFile = "Assets/Futa/Textures/NileV1.png";
        private const string textureMistyFile = "Assets/Futa/Textures/MistyV1.png";

        private const string spriteHContentFile = "Assets/Futa/Textures/IntersexButtonBG.png";

        private const string appendageGenericPrefabFile = "Assets/Futa/Prefs/AppendageGeneric.prefab";
        private const string appendageAriPrefabFile = "Assets/Futa/Prefs/AppendageAri.prefab";
        private const string appendageMistyPrefabFile = "Assets/Futa/Prefs/AppendageMisty.prefab";

        private const string creamScreenPrefabFile = "Assets/Futa/Prefs/CreamScreen.prefab";

        private Texture2D textureSammy;
        private Texture2D textureMaddie;
        private Texture2D texturePoppi;
        private Texture2D textureAri;
        private Texture2D textureNile;
        private Texture2D textureMisty;

        private Sprite spriteHContent;

        private static List<CharacterControllerBase> activeCharacters;

        private GameObject prefabCreamScreen;

        private GameObject activeCreamScreen;

        private GameObject prefabAppendageGeneric;
        private GameObject prefabAppendageAri;
        private GameObject prefabAppendageMisty;

        Material defaultMaterial;
        Shader defaultShader;

        private RectTransform adultSection;

        private static ModMain instance;

        private Button creamButton;

        LevelManagerBase levelManager;

        bool looseScreenCreaming;

        public static bool adult { get; private set; } = false;
        public static bool topless { get; private set; } = false;
        public static event Action<bool> OnToplessChange;
        public static bool intersex { get; private set; } = false;
        public static event Action<bool> OnIntersexChange;

        Characters attackedCharacter;





        #region Initialisation




        public override void OnInitializeMelon() => LoadAssetBundle();



        void LoadAssetBundle()
        {
            MemoryStream memoryStream;  //Regular asset bundle loading

            using (Stream stream = MelonAssembly.Assembly.GetManifestResourceStream(addressField))
            {
                memoryStream = new MemoryStream((int)stream.Length);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                memoryStream.Write(buffer, 0, buffer.Length);
            }

            AssetBundle _bundle = AssetBundle.LoadFromMemory(memoryStream.ToArray());
             
            //Assign assets to variables

            prefabCreamScreen = _bundle.LoadAsset_Internal(creamScreenPrefabFile, Il2CppType.Of<GameObject>()).Cast<GameObject>();

            prefabAppendageGeneric = _bundle.LoadAsset_Internal(appendageGenericPrefabFile, Il2CppType.Of<GameObject>()).Cast<GameObject>();
            prefabAppendageAri = _bundle.LoadAsset_Internal(appendageAriPrefabFile, Il2CppType.Of<GameObject>()).Cast<GameObject>();
            prefabAppendageMisty = _bundle.LoadAsset_Internal(appendageMistyPrefabFile, Il2CppType.Of<GameObject>()).Cast<GameObject>();

            texturePoppi = _bundle.LoadAsset_Internal(texturePoppiFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureNile = _bundle.LoadAsset_Internal(textureNileFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureSammy = _bundle.LoadAsset_Internal(textureSammyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAri = _bundle.LoadAsset_Internal(textureAriFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureMaddie = _bundle.LoadAsset_Internal(textureMaddieFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureMisty= _bundle.LoadAsset_Internal(textureMistyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();

            Texture2D _spriteTemp = _bundle.LoadAsset_Internal(spriteHContentFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            spriteHContent = Sprite.Create(_spriteTemp, new Rect(0, 0, _spriteTemp.width, _spriteTemp.height), Vector2.one * .5f, 100);

            //Add flags, so the variables won't be cleared on scene change

            prefabCreamScreen.hideFlags = HideFlags.DontUnloadUnusedAsset;

            prefabAppendageGeneric.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            prefabAppendageAri.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            prefabAppendageMisty.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            texturePoppi.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureNile.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureSammy.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAri.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureMaddie.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureMisty.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            spriteHContent.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }



        void FetchDefaultAssets()
        {
            adult = SaveManager.Settings.HContent;
            ApplyAdultToggles();
//            intersex = (PlayerPrefs.GetString("Intersex") == "True");
//            topless = (PlayerPrefs.GetString("Topless") == "True");

            if (defaultShader)
                return;

            if (!GameObject.FindObjectOfType<SkinnedMeshRenderer>())
                return;

            defaultShader = GameObject.FindObjectOfType<SkinnedMeshRenderer>().material.shader;
            defaultShader.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }



        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            instance = this;
            FetchDefaultAssets();

            activeCharacters = new List<CharacterControllerBase>(0) { };
            activeAppendages = new List<Appendage>(0) { };

            if (instance.activeCreamScreen)
                UnityEngine.Object.Destroy(instance.activeCreamScreen);

            looseScreenCreaming = false;

            if (LevelManagerBase.Exists)
                levelManager = LevelManagerBase.Instance;
            else                                                                                    //If main menu
            if (GameObject.FindObjectOfType<AdultSpriteSwitcher>())                     //If Gallery button found
            {
                AdultSpriteSwitcher _ad = GameObject.FindObjectOfType<AdultSpriteSwitcher>();       //Assign Gallery BG switcher    
                _ad._nsfwSprite = spriteHContent;                                                   //Assign new gallery BG image
                _ad.UpdateSprite(SaveManager.LoadSettings().HContent);//Set/Update current state
            }
        }




        #endregion




        #region Appendage




        public void InstantiateAppendage(CharacterControllerBase _character, Transform _hipBone)
        {
            GameObject _appendage;

            switch (_character.character)
            {
                case Characters.Ari:
                _appendage = UnityEngine.Object.Instantiate(prefabAppendageAri, _hipBone);
                    break;

                case Characters.Misty:
                    _appendage = UnityEngine.Object.Instantiate(prefabAppendageMisty, _hipBone);
                    break;

                default:
                _appendage = UnityEngine.Object.Instantiate(prefabAppendageGeneric, _hipBone);
                    break;
            }

            SkinnedMeshRenderer _mesh = _appendage.transform.GetComponentInChildren<SkinnedMeshRenderer>();
            Animator _anim = _appendage.GetComponent<Animator>();

            _mesh.material = defaultMaterial;                       //Assign default params
            _mesh.material.shader = defaultShader;                  //Assign default params
            _mesh.material.SetFloat("_Smoothness", 0);              //Remove shine
            _mesh.material.SetFloat("_EnvironmentReflections", 0);  //Remove shine

            switch (_character.character)
            {
                case Characters.Ari:
                    _mesh.material.mainTexture = textureAri;
                    SetAppendageParameters(_anim, .103f, .49f, 1, 1);
                    break;  //Green

                case Characters.Sammy:
                    _mesh.material.mainTexture = textureSammy;
                    SetAppendageParameters(_anim, .065f, .68f, .3f, .16f);
                    break;  //Tiger

                case Characters.Poppi:
                    _mesh.material.mainTexture = texturePoppi;
                    SetAppendageParameters(_anim, .071f, .66f, 0, .1f);
                    break;  //Buni

                case Characters.Nile:
                    _mesh.material.mainTexture = textureNile;
                    SetAppendageParameters(_anim, .091f, .55f, 0, .45f);
                    break;  //Black cat

                case Characters.Maddie:
                    _mesh.material.mainTexture = textureMaddie;
                    SetAppendageParameters(_anim, .13f, .7f, .7f);
                    break;  //Microvawe thing

                case Characters.Misty:
                    _mesh.material.mainTexture = textureMisty;
                    SetAppendageParameters(_anim, .1f, .5f, 1);
                    break;  //Shark

                case Characters.Kass:
                    UnityEngine.Object.Destroy(_appendage);
                    return; //Kass is not an actual enemy character (racoon dumpster thing)

                case Characters.AriBM:
                    UnityEngine.Object.Destroy(_appendage);
                    return;
            }

            MelonCoroutines.Start(AppendageStart(_mesh, _character));   //Start appendage routine for a fresh new appendage
        }



        /// <summary>
        /// Set Placement, Scale and overall look.
        /// </summary>
        /// <param name="_anim">Animator</param>
        /// <param name="_offest">How much the appendage should be moved forward (locally)</param>
        /// <param name="_scale">Scale of appendage's GameObject</param>
        /// <param name="_fertility">Determens the size of Two Male Bump. Removes them completely if 0</param>
        /// <param name="_diameter">Additional diameter of an appendage (stacks with _scale)</param>
        void SetAppendageParameters(Animator _anim, float _offest = .068f, float _scale = .65f, float _diameter = .3f, float _fertility = 0)
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
        private IEnumerator AppendageStart(SkinnedMeshRenderer _mesh, CharacterControllerBase _controller)
        {

            Animator _anim = _mesh.transform.parent.GetComponent<Animator>();

            _anim.SetFloat("Breathe", UnityEngine.Random.Range(.7f, 2f)); //Desync cletching speed

            _anim.SetBool("Intersex", intersex && SaveManager.Settings.HContent);                            //Assign current settings

            _anim.SetBool("Topless", topless);                              //Assign current settings

            activeAppendages.Add(new Appendage(_mesh.transform.parent.gameObject, _controller.character, _anim));

            if (topless)
                _controller.CostumeSwitcher.SwitchVariant("Nude");      //Undress character

            switch (LevelManagerBase.Level)
            {
                case Level.SecurityOffice:

                    SecurityOfficeCharacterController _officeController = _controller.GetComponent<SecurityOfficeCharacterController>();
                    _anim.SetFloat("Enlarge", .5f);
                    while (_mesh)                                       //Cycle while appendage eхists
                    {
                        //MelonLogger.Msg(_controller.character + " @: " + _officeController.currentPosition);
                        yield return new WaitForSeconds(1);
                        break;
                    }
                    break;

                case Level.Nightclub:

                    NightclubCharacterController _clubController = _controller.GetComponent<NightclubCharacterController>();
                    while (_mesh)                                       //Cycle while appendage eхists
                    {
                        _anim.SetFloat("Enlarge", _clubController._excitement._excitement);
                        _anim.SetBool("Leak", _clubController.IsExcited);

                        yield return new WaitForSeconds(.2f);           //Artificial value, can be 1s, but .2 for the smooth Enlarge animation
                    }
                    break;
            }

        }



        /// <summary>
        /// Put appendage into a Fun Time mode
        /// </summary>Character</param>
        /// <param name="_fun">Are we having a good time? Or we done?</param>
        public static void AppendageActionOnRuntime(Characters _char, bool _leak = false, bool _cream = false, float _enlarge = -1)
        {
            if (_char == Characters.Everyone)
                foreach (Appendage _appendage in activeAppendages)
                {
                    _appendage._Animator.SetBool("FunTime", _cream);                  //Set _fun to every character on a level
                    _appendage._Animator.SetBool("Leak", _leak);
                    if (_enlarge != -1)
                        _appendage._Animator.SetFloat("Enlarge", _enlarge);
                }
            else
            {
                int index = activeAppendages.FindIndex(x => x._Character == _char);     //Honesstly, idk, just copy/pasted from SO, and it works :p
                if (index >= 0)
                {
                    activeAppendages[index]._Animator.SetBool("FunTime", _cream);     //Set _fun to a speciffic character's appendage
                    activeAppendages[index]._Animator.SetBool("Leak", _leak);
                    if (_enlarge != -1)
                        activeAppendages[index]._Animator.SetFloat("Enlarge", _enlarge);
                                        
                    MelonCoroutines.Start(HandleCreamScreen(_cream, _char));   //Start appendage routine for a fresh new appendage
                }
            }
        }



        private static IEnumerator HandleCreamScreen(bool _cream, Characters _character)
        {
            if (!instance.levelManager)
                yield break;

            if (_cream)
            {
                if (instance.activeCreamScreen)
                    yield break;
            }
            else
            {
                if (instance.activeCreamScreen)
                    UnityEngine.Object.Destroy(instance.activeCreamScreen);
                yield break;
            }

            bool _cameraCream = false;

            switch (LevelManagerBase.Level)
            {
                case Level.SecurityOffice:
                    switch (_character)
                    {
                        case Characters.Sammy:
                            break;
                        case Characters.Nile:
                            _cameraCream = true;
                            break;
                        case Characters.Poppi:
                            _cameraCream = true;
                            break;
                        case Characters.Misty:
                            break;
                        case Characters.Maddie:
                            break;
                        case Characters.Ari:
                            break;
                        case Characters.Kass:
                            break;
                        case Characters.AriBM:
                            break;
                        default:
                            break;
                    }
                    break;

                case Level.Nightclub:
                    switch (_character)
                    {
                        case Characters.Sammy:
                            _cameraCream = true;
                            break;
                        case Characters.Nile:
                            _cameraCream = true;
                            break;
                        case Characters.Poppi:
                            _cameraCream = true;
                            break;
                        case Characters.Misty:
                            break;
                        case Characters.Maddie:
                            break;
                        case Characters.Ari:
                            break;
                        case Characters.Kass:
                            break;
                        case Characters.AriBM:
                            break;
                        default:
                            break;
                    }
                    break;
            }

            if (_cameraCream)
                instance.activeCreamScreen = UnityEngine.Object.Instantiate(instance.prefabCreamScreen, Camera.main.transform);   //Spawm Splooge screen
            yield return null;
        }


        #endregion




        #region Settings




        /// <summary>
        /// Creates a generic toggle in the Adult section of Settings.
        /// </summary>
        /// <param name="_title">Title teхt for toggle in settings</param>
        /// <param name="_void">Toggle action</param>
        /// <param name="_loadState">Sets toggles' initial state on spawn</param>
        public static void InstantiateToggleInSettings(string _title, Action<bool> _void, bool _loadState)
        {
            GameObject _toggle = UnityEngine.Object.Instantiate(GameObject.Find("Radio - H Content"), instance.adultSection);    //Get and spawn a generic toggle

            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontEvent>());             //Remove translation junk
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontMaterialEvent>());     //Remove translation junk
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeStringEvent>());              //Remove translation junk

            _toggle.GetComponentInChildren<TextMeshProUGUI>().text = _title;                    //Set toggle title teхt
            _toggle.GetComponent<UIRadio>().SetRadio(_loadState, false);                                    //Set initial state for toggle
            _toggle.GetComponent<UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(_void));  //Assign listener to a state change
        }



        void SpawnCustomSettingItems()
        {
            if (adultSection)       //Check if container is assigned
                return;         //Don't do anything if it does

            GameObject _radio = GameObject.Find("Radio - H Content");                   //Find a toogle in Adult section and use it as a base for mine
            _radio.GetComponent<UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(AddAdultToggleListener));  //Assign listener to a state change
            adultSection = _radio.transform.parent.GetComponent<RectTransform>();       //Get and assign a container in Settings that stores all the Adult stuff
            InstantiateToggleInSettings("Intersex Mode", AddIntersexToggleListener, (PlayerPrefs.GetString("Intersex") == "True"));  //Spawn Interseх toggle
            InstantiateToggleInSettings("Topless Mode", AddToplessToggleListener, (PlayerPrefs.GetString("Topless") == "True"));     //Spawn Topless toggle
        }


        void ApplyAdultToggles()
        {
            intersex = (PlayerPrefs.GetString("Intersex") == "True") && adult;

            topless = (PlayerPrefs.GetString("Topless") == "True") && adult;         //Set Topless global bool


            foreach (Appendage _appendage in activeAppendages)
                _appendage._Animator.SetBool("Intersex", intersex);            //Apply it to appendages


            foreach (Appendage _appendage in activeAppendages)
                _appendage._Animator.SetBool("Topless", topless);             //Apply it to appendages

            if (LevelManagerBase.Exists)
            {
                if (topless)
                    foreach (CharacterControllerBase item in activeCharacters)
                        item.CostumeSwitcher.SwitchVariant("Nude");             //Undress everyone
                else
                    foreach (CharacterControllerBase item in activeCharacters)
                        item.CostumeSwitcher.SwitchVariant("Clothed");          //Everyone, put the clothes on!
            }

        }



        void AddAdultToggleListener(bool _isOn)
        {
            adult = _isOn;

            ApplyAdultToggles();
        }



        void AddIntersexToggleListener(bool _isOn)
        {
            PlayerPrefs.SetString("Intersex", _isOn.ToString());            //Save state of toggle

            ApplyAdultToggles();
        }



        void AddToplessToggleListener(bool _isOn)
        {
            PlayerPrefs.SetString("Topless", _isOn.ToString());             //Save state of toggle

            ApplyAdultToggles();
        }



        void AddLooseButtonListener()
        {
            if (creamButton)      //Check if options button is assigned
                return;         //Don't do anything if it does

            GameObject genericButton = GameObject.Find("Button - Hide UI");
            creamButton = GameObject.Instantiate(genericButton.GetComponent<Button>(), genericButton.transform.parent);        //Get and assign a Settings button from the menu
            creamButton.GetComponent<RectTransform>().localPosition += new Vector3(0,130,0);

            UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeTMPFontEvent>());             //Remove translation junk
            UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeTMPFontMaterialEvent>());     //Remove translation junk
            UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeStringEvent>());              //Remove translation junk

            creamButton.GetComponentInChildren<Il2CppTMPro.TextMeshProUGUI>().text = "Toggle Cream";                    //Set toggle title teхt
            creamButton.onClick.AddListener(new Action(() => { CreamToggle(); }));                              //Add listener to button pressed
        }


        void CreamToggle()
        {
            looseScreenCreaming  = !looseScreenCreaming;
            AppendageActionOnRuntime(attackedCharacter, false, looseScreenCreaming);
        }


        #endregion




        #region Patching Characters




        /// <summary>
        /// Add custom code to Funtime scene start
        /// "OnAttackTransitionEnd" is being called after the fade in*
        /// </summary>
        [HarmonyPatch(typeof(NightclubCharacterController), "OnAttackTransitionEnd")]
        private static class ActStart   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Prefix(ref NightclubCharacterController __instance)  //Call stuff before original script's events accure
            => AppendageActionOnRuntime(__instance.character, false, true);                                //Start creaming
        }
        

        /// <summary>
        /// Add custom code to Funttime scene End
        /// "BeforeTimeSkip" is being called after the Fun scene's fade in
        /// </summary>
        [HarmonyPatch(typeof(NightclubCharacterController), "BeforeTimeSkip")]
        private static class ActEnd   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Postfix(ref NightclubCharacterController __instance)  //Call stuff before original script's events accure
            {
                if (topless)                                        //Check if Topless toggle is on
                    __instance.CostumeSwitcher.SwitchVariant("Nude");           //Undress the character

                AppendageActionOnRuntime(__instance.character, false, false);                               //Stop creaming
            }
        }


        /// <summary>
        /// Add custom code to Funttime scene End
        /// "BeforeTimeSkip" is being called after the Fun scene's fade in
        /// </summary>
        [HarmonyPatch(typeof(NightclubCharacterController), "MoveToRandom")]
        private static class RandomMove   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Postfix(ref NightclubCharacterController __instance)  //Call stuff before original script's events accure
            {
                if (topless)                                        //Check if Topless toggle is on
                    __instance.CostumeSwitcher.SwitchVariant("Nude");           //Undress the character
            }
        }



        [HarmonyPatch(typeof(SecurityOfficeCharacterController), "AttackPlayer")]
        private static class CharacterMovemed   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Postfix(ref SecurityOfficeCharacterController __instance)    //Original script patch + character _instance that had called it
            {
                instance.attackedCharacter = __instance.character;
                AppendageActionOnRuntime(__instance.character, false, false, 1);
            }
        }


        [HarmonyPatch(typeof(SettingsUIManager), "Awake")]
        private static class Pauseddddddd   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Postfix(ref SettingsUIManager __instance)    //Original script patch + character _instance that had called it
            => instance.SpawnCustomSettingItems();
        }



        [HarmonyPatch(typeof(GameResultLosePanel), "ShowPanel")]
        private static class Ovdsda   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Postfix(ref GameResultLosePanel __instance)    //Original script patch + character _instance that had called it
            => instance.AddLooseButtonListener();
        }




        [HarmonyPatch(typeof(CharacterControllerBase), "Initialize")]
        private static class CharacterInit   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Postfix(ref CharacterControllerBase __instance)  //Call stuff before original script's events accure
            {
                activeCharacters.Add(__instance);                               //Add this character to active characters

                Transform[] _armatureBones = __instance.Animator.transform.GetChild(0).GetComponentsInChildren<Transform>();    //Get all the bones in the armature
                foreach (Transform _bone in _armatureBones)
                    if (_bone.name == "DEF-spine")  //Not effective, but just in case if character's hierarchy gets messed up. Animator.Avatar / .GetBoneTra returns null 
                    {
                        FutanariMod.ModMain.instance.InstantiateAppendage(__instance, _bone.transform);  //Instantiate appendage in Hip bone
                        break;
                    }
            }
        }



        #endregion




        /// <summary>
        /// Pause check
        /// </summary>
        public override void OnLateUpdate()
        {
            //DBG
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                MelonLogger.Msg("TP " + activeCharacters.Count + " characters");
                byte id = 0;
                foreach (CharacterControllerBase _obj in activeCharacters)
                {
                    _obj.transform.rotation = Quaternion.identity;
                    _obj.transform.position = Camera.main.transform.position + Camera.main.transform.right * (id - 2) + Camera.main.transform.forward * 3 + Camera.main.transform.up * -.6f;
                    id++;
                }



            }

        }

    }
}
