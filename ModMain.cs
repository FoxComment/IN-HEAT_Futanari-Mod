using MelonLoader;
using UnityEngine;
using FutanariMod;
using Il2CppMonsterBox;
using System.Collections;
using UnityEngine.UI;
using Il2CppInterop.Runtime;
using Il2CppMonsterBox.Runtime.Extensions._Localization;
using Il2CppMonsterBox.Runtime.Gameplay.Nightclub.Character;
using Il2CppMonsterBox.Systems.Tools.Miscellaneous.Adult;
using Il2CppMonsterBox.Runtime.UI.Settings;
using Il2CppMonsterBox.Runtime.Gameplay.Enums;
using Il2CppMonsterBox.Runtime.Gameplay.SecurityOffice;
using Il2CppMonsterBox.Runtime.Gameplay.Character;
using Il2CppMonsterBox.Systems.UI.Elements;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using HarmonyLib;
using Il2CppMonsterBox.Runtime.Gameplay.Level;
using Il2CppTMPro;
using Il2CppMonsterBox.Runtime.UI.Game.Result;
using Il2CppMonsterBox.Systems.Saving;

[assembly: MelonInfo(typeof(ModMain), "Futanari Mod", "2024.9.30", "FoxComment", "https://github.com/foxcomment/IN-HEAT_Futanari-Mod")]
[assembly: MelonGame("MonsterBox", "IN HEAT")]
namespace FutanariMod
{
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

        public static bool adult { get; private set; } = true;
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
            adult = SaveManager.LoadSettings().HContent;
            
            ApplyAdultToggles();

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

            activeCharacters = new List<CharacterControllerBase>(0) { };
            activeAppendages = new List<Appendage>(0) { };

            if (instance.activeCreamScreen)
                UnityEngine.Object.Destroy(instance.activeCreamScreen);

            looseScreenCreaming = false;

            if (LevelManagerBase.Exists)
                levelManager = LevelManagerBase.Instance;

            FetchDefaultAssets();
        }




        #endregion




        #region Appendage




        public void InstantiateAppendage(CharacterControllerBase _character, Transform _hipBone)
        {
            GameObject _appendage;

            switch (_character.character)   //Instntiate a speciffic appendage model
            {
                case Characters.Ari:    //Canine
                _appendage = UnityEngine.Object.Instantiate(prefabAppendageAri, _hipBone);
                    break;

                case Characters.Misty:  //Tapered
                    _appendage = UnityEngine.Object.Instantiate(prefabAppendageMisty, _hipBone);
                    break;

                default:    //Generic
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
                    SetupAppendage(_anim, .103f, .49f, 1, 1);
                    break;  //Green

                case Characters.Sammy:
                    _mesh.material.mainTexture = textureSammy;
                    SetupAppendage(_anim, .065f, .68f, .3f, .16f);
                    break;  //Tiger

                case Characters.Poppi:
                    _mesh.material.mainTexture = texturePoppi;
                    SetupAppendage(_anim, .071f, .66f, 0, .1f);
                    break;  //Buni

                case Characters.Nile:
                    _mesh.material.mainTexture = textureNile;
                    SetupAppendage(_anim, .091f, .55f, 0, .45f);
                    break;  //Black cat

                case Characters.Maddie:
                    _mesh.material.mainTexture = textureMaddie;
                    SetupAppendage(_anim, .13f, .7f, .7f);
                    break;  //Microvawe thing

                case Characters.Misty:
                    _mesh.material.mainTexture = textureMisty;
                    SetupAppendage(_anim, .1f, .5f, 1);
                    break;  //Shark

                case Characters.Kass:
                    UnityEngine.Object.Destroy(_appendage);
                    return; //Kass is not an actual enemy character (racoon dumpster thing)

                case Characters.AriBM:
                    UnityEngine.Object.Destroy(_appendage);
                    return;
            }

            MelonCoroutines.Start(AppendageLifeCycle(_mesh, _character));   //Start appendage routine for a fresh new appendage
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
        private IEnumerator AppendageLifeCycle(SkinnedMeshRenderer _mesh, CharacterControllerBase _controller)
        {
            Animator _anim = _mesh.transform.parent.GetComponent<Animator>();

            _anim.SetFloat("Breathe", UnityEngine.Random.Range(.7f, 2f));            //Desync cletching speed
                                                                                     
            _anim.SetBool("Intersex", intersex && SaveManager.Settings.HContent);    //Assign current settings
                                                                                     
            _anim.SetBool("Topless", topless);                                       //Assign current settings

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
                     //   yield return new WaitForSeconds(1);
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
            if (!instance.levelManager) //Check if we're on an actual level
                yield break;        //Stop if not

            if (_cream)
            {
                if (instance.activeCreamScreen) //Check if there's already a cream screen
                    yield break;            //Stop if there is
            }
            else            //Stop creaming
            {
                if (instance.activeCreamScreen)
                    UnityEngine.Object.Destroy(instance.activeCreamScreen);
                yield break;
            }
            
            bool _hipFacingCamera = false;      //Chaaracter's hip facing a camera

            switch (LevelManagerBase.Level)     //Characters have different animaations, depending on a level
            {
                case Level.SecurityOffice:
                    switch (_character)
                    {
                        case Characters.Sammy:
                            break;
                        case Characters.Nile:
                            _hipFacingCamera = true;
                            break;
                        case Characters.Poppi:
                            _hipFacingCamera = true;
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
                            _hipFacingCamera = true;
                            break;
                        case Characters.Nile:
                            _hipFacingCamera = true;
                            break;
                        case Characters.Poppi:
                            _hipFacingCamera = true;
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

            if (_hipFacingCamera)
                instance.activeCreamScreen = UnityEngine.Object.Instantiate(instance.prefabCreamScreen, Camera.main.transform);   //Spawm Splooge screen
            yield return null;
        }
      



        #endregion




        #region UI




        void SpawnCreamButton()
        {
            if (creamButton || !intersex)      //Check if Creaming button already eхists or Interseх mode is off
                return;

            GameObject genericButton = GameObject.Find("Button - Hide UI");
            creamButton = GameObject.Instantiate(genericButton.GetComponent<Button>(), genericButton.transform.parent);        //Get and assign a Settings button from the menu
            creamButton.GetComponent<RectTransform>().localPosition += new Vector3(0, 130, 0);

            UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeTMPFontEvent>());             //Remove translation junk
            UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeTMPFontMaterialEvent>());     //Remove translation junk
            UnityEngine.Object.Destroy(creamButton.GetComponentInChildren<LocalizeStringEvent>());              //Remove translation junk

            creamButton.GetComponentInChildren<Il2CppTMPro.TextMeshProUGUI>().text = "Toggle Cream";                    //Set toggle title teхt
            creamButton.onClick.AddListener(new Action(() => { CreamButtonListener(); }));                              //Add listener to button pressed
        }



        void SpawnCustomSettingItems()
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
        /// Creates a generic toggle in the Adult section of Settings.
        /// </summary>
        /// <param name="_title">Title teхt for toggle in settings</param>
        /// <param name="_void">Toggle action</param>
        /// <param name="_initialState">Set toggles' initial state on spawn</param>
        public static void InstantiateToggleInSettings(string _title, Action<bool> _void, bool _initialState)
        {
            GameObject _toggle = UnityEngine.Object.Instantiate(GameObject.Find("Radio - H Content"), instance.adultSection);    //Get and spawn a generic toggle

            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontEvent>());             //Remove translation junk
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontMaterialEvent>());     //Remove translation junk
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeStringEvent>());              //Remove translation junk

            _toggle.GetComponentInChildren<TextMeshProUGUI>().text = _title;                    //Set toggle title teхt
            _toggle.GetComponent<UIRadio>().SetRadio(_initialState, false);                                    //Set initial state for toggle
            _toggle.GetComponent<UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(_void));  //Assign listener to a state change
        }



        void ApplyAdultToggles()
        {
            intersex = (PlayerPrefs.GetString("Intersex") == "True") && adult;      //Check if Intersex and H-Content toggles are active

            topless = (PlayerPrefs.GetString("Topless") == "True") && adult;        //Check if Topless and H-Content toggles are active

            foreach (Appendage _appendage in activeAppendages)
                _appendage._Animator.SetBool("Intersex", intersex);            


            foreach (Appendage _appendage in activeAppendages)
                _appendage._Animator.SetBool("Topless", topless);             

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



        void AdultToggleListener(bool _isOn)
        {
            adult = _isOn;

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



        void CreamButtonListener()
        {
            looseScreenCreaming  = !looseScreenCreaming;
            AppendageActionOnRuntime(attackedCharacter, false, looseScreenCreaming);
        }




        #endregion

        
        

        #region Patching Characters




        /// <summary>
        /// Being called after the fade in from the door enterense sequence
        /// </summary>
        [HarmonyPatch(typeof(NightclubCharacterController), "OnAttackTransitionEnd")]
        private static class ClubActStart   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Prefix(ref NightclubCharacterController __instance)  //Call stuff before original script's events accure
            => AppendageActionOnRuntime(__instance.character, false, true);                                //Start creaming
        }
        


        /// <summary>
        /// Being called after the fade in after the Attack Scene had ended
        /// </summary>
        [HarmonyPatch(typeof(NightclubCharacterController), "BeforeTimeSkip")]
        private static class ClubActEnd 
        {
            public static void Postfix(ref NightclubCharacterController __instance)  //Call stuff before original script's events accure
            {
                if (topless)                                        //Check if Topless toggle is on
                    __instance.CostumeSwitcher.SwitchVariant("Nude");           //Undress the character

                AppendageActionOnRuntime(__instance.character, false, false);                               //Stop creaming
            }
        }



        /// <summary>
        /// Sitches character to Nude after having a fun time with a customer
        /// </summary>
        [HarmonyPatch(typeof(NightclubCharacterController), "MoveToRandom")]
        private static class ClubCustomerActEnd   
        {
            public static void Postfix(ref NightclubCharacterController __instance)
            {
                if (topless)                                        //Check if Topless toggle is on
                    __instance.CostumeSwitcher.SwitchVariant("Nude");           //Undress the character
            }
        }



        [HarmonyPatch(typeof(SecurityOfficeCharacterController), "AttackPlayer")]
        private static class CharacterMovemed 
        {
            public static void Postfix(ref SecurityOfficeCharacterController __instance)    //Original script patch + character _instance that had called it
            {
                instance.attackedCharacter = __instance.character;
                AppendageActionOnRuntime(__instance.character, false, false, 1);
            }
        }


        [HarmonyPatch(typeof(SettingsUIManager), "Awake")]
        private static class Pauseddddddd  
        {
            public static void Postfix(ref SettingsUIManager __instance) 
            => instance.SpawnCustomSettingItems();
        }
        


        /// <summary>
        /// Game Over screen in Office
        /// </summary>
        [HarmonyPatch(typeof(GameResultLosePanel), "ShowPanel")]
        private static class GameOverScreenShowed
        {
            public static void Postfix(ref GameResultLosePanel __instance)
            => instance.SpawnCreamButton();
        }


        /// <summary>
        /// Game Over screen in Office
        /// </summary>
        [HarmonyPatch(typeof(AdultSpriteSwitcher), "Awake")]
        private static class ChangeDefaultAdultGalleryBG
        {
            public static void Prefix(ref AdultSpriteSwitcher __instance)
            => __instance._nsfwSprite = instance.spriteHContent;
        }



        /// <summary>
        /// Being called on character spawn
        /// </summary>
        [HarmonyPatch(typeof(CharacterControllerBase), "Initialize")]
        private static class CharacterInit
        {
            public static void Postfix(ref CharacterControllerBase __instance) 
            {
                activeCharacters.Add(__instance);                               //Add this character to active characters

                Transform[] _armatureBones = __instance.Animator.transform.GetChild(0).GetComponentsInChildren<Transform>();    //Get all the bones in the armature
                foreach (Transform _bone in _armatureBones)
                    if (_bone.name == "DEF-spine")  //Not effective, but just in case if character's hierarchy gets messed up. Animator.Avatar / .GetBoneTra returns null 
                    {
                        instance.InstantiateAppendage(__instance, _bone.transform);  //Instantiate appendage in Hip bone
                        break;
                    }
            }
        }




        #endregion




    }
}
