using MelonLoader;
using UnityEngine;
using FutaMod;
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
using Il2CppNodeCanvas.Tasks.Actions;
using Mono.CSharp;
using UnityEngine.TextCore.Text;
using UnityEngine.Rendering.Universal;
using Il2CppSteamworks;
using static FutaMod.ModMain;
[assembly: MelonInfo(typeof(ModMain), "FutanariMod", "10.10.2024", "FoxComment", "https://github.com/foxcomment/inheat_futamod")]
[assembly: MelonGame("MonsterBox", "IN HEAT")]
namespace FutaMod
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
        private const string textureMaddyFile = "Assets/Futa/Textures/MaddyV1.png";
        private const string texturePoppiFile = "Assets/Futa/Textures/PoppiV1.png";
        private const string textureAriFile = "Assets/Futa/Textures/AriV1.png";
        private const string textureNileFile = "Assets/Futa/Textures/NileV1.png";

        private const string spriteHContentFile = "Assets/Futa/Textures/IntersexButtonBG.png";

        private const string appendageOnePrefabFile = "Assets/Futa/Prefs/AppengadeOne.prefab";

        private Texture2D textureSammy;
        private Texture2D textureMaddy;
        private Texture2D texturePoppi;
        private Texture2D textureAri;
        private Texture2D textureNile;

        private Sprite spriteHContent;

        private CharacterControllerBase[] activeCharacters;

        private GameObject prefabAppendageOne;
        //GameObject prefabAppendageTwo;        //ADD MORE MODELS
        //GameObject prefabAppendageThree;      //ADD MORE MODELS

        Material defaultMaterial;
        Shader defaultShader;

        private RectTransform adultSection;

        private Button optionsButton;

        bool DEBUG = true;

        public static bool topless { get; private set; } = false;   //Turn into an event

        public static bool intersex { get; private set; } = false;   //Turn into an event





        #region Initialisation




        public override void OnInitializeMelon() => LoadAssetBundle();
        void FetchCharacters() => MelonCoroutines.Start(fetchCharacters());



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

            if (DEBUG)
            {
                MelonLogger.Msg("*=-  " + _bundle.name + "   \n\n Listing:");

                string[] objNames = _bundle.GetAllAssetNames();
                foreach (string objName in objNames)
                    MelonLogger.Msg("   * " + objName);
            }

            //Assign assets to variables

            prefabAppendageOne = _bundle.LoadAsset_Internal(appendageOnePrefabFile, Il2CppType.Of<GameObject>()).Cast<GameObject>();

            texturePoppi = _bundle.LoadAsset_Internal(texturePoppiFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureNile = _bundle.LoadAsset_Internal(textureNileFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureSammy = _bundle.LoadAsset_Internal(textureSammyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAri = _bundle.LoadAsset_Internal(textureAriFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureMaddy = _bundle.LoadAsset_Internal(textureMaddyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();

            Texture2D _spriteTemp = _bundle.LoadAsset_Internal(spriteHContentFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            spriteHContent = Sprite.Create(_spriteTemp, new Rect(0, 0, _spriteTemp.width, _spriteTemp.height), Vector2.one * .5f, 100);

            //Add flags, so the variables won't be cleared on scene change

            prefabAppendageOne.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            texturePoppi.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureNile.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureSammy.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAri.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureMaddy.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            spriteHContent.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }



        void FetchDefaultAssets()
        {
            intersex = (PlayerPrefs.GetString("Intersex") == "True");
            topless = (PlayerPrefs.GetString("Topless") == "True");

            if (defaultShader)
                return;

            if (GameObject.FindObjectOfType<SkinnedMeshRenderer>() == null)
                return;

            defaultShader = GameObject.FindObjectOfType<SkinnedMeshRenderer>().material.shader;
            defaultShader.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }



        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            FetchDefaultAssets();

            activeCharacters = new CharacterControllerBase[0];
            activeAppendages = new List<Appendage>(0) { };

            if (LevelManagerBase.Exists)
                switch (LevelManagerBase.Level)
                {
                    case Level.SecurityOffice:
                        FetchCharacters();
                        break;
                    case Level.Nightclub:
                        FetchCharacters();
                        break;
                }
            else                                                                                    //If main menu
            if (GameObject.FindObjectOfType<AdultSpriteSwitcher>())                     //If Gallery button found
            {
                AdultSpriteSwitcher _ad = GameObject.FindObjectOfType<AdultSpriteSwitcher>();       //Assign Gallery BG switcher    
                _ad._nsfwSprite = spriteHContent;                                                   //Assign new gallery BG image
                _ad.UpdateSprite(Il2CppMonsterBox.Systems.Saving.SaveManager.LoadSettings().HContent);//Set/Update current state

                AddSettingsButtonListener();
            }
        }



        IEnumerator fetchCharacters()
        {
            //Give this thing a moment, because characters are not being spaned on a first frame for whatever reason
            yield return new WaitForSeconds(.1f);

            activeCharacters = GameObject.FindObjectsOfType<CharacterControllerBase>(); //Get all the Characters on the level

            yield return new WaitForSeconds(.1f);
            SpawnAppendagesForActiveChatacters();
        }




        #endregion




        #region Appendage




        void SpawnAppendagesForActiveChatacters()
        {
            foreach (CharacterControllerBase _character in activeCharacters)
            {
                var children = _character.Animator.transform.GetChild(0).GetComponentsInChildren<Transform>();
                foreach (var child in children)
                    if (child.name == "DEF-spine")
                    {
                        InstantiateAppendage(_character, child.transform);
                        break;
                    }
            }
        }



        private void InstantiateAppendage(CharacterControllerBase _character, Transform _hipBone)
        {
            GameObject _appendage = UnityEngine.Object.Instantiate(prefabAppendageOne, _hipBone);
            SkinnedMeshRenderer _mesh = _appendage.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
            Animator _anim = _appendage.GetComponent<Animator>();

            _mesh.material = defaultMaterial;                       //Assign default params
            _mesh.material.shader = defaultShader;                  //Assign default params
            _mesh.material.SetFloat("_Smoothness", 0);              //Remove shine
            _mesh.material.SetFloat("_EnvironmentReflections", 0);  //Remove shine

            switch (_character.character) 
            {
                case Characters.Ari:
                    _mesh.material.mainTexture = textureAri;
                    SetAppendageParameters(_anim, .103f, .49f, 0, .02f);
                    break;  //Green

                case Characters.Sammy:
                    _mesh.material.mainTexture = textureSammy;
                    SetAppendageParameters(_anim, .065f, .68f,.3f,.16f);
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
                    _mesh.material.mainTexture = textureMaddy;
                    SetAppendageParameters(_anim, .13f, .7f, .7f, .35f);
                    break;  //Microvawe thing
                
                case Characters.Misty:
                    _mesh.material.mainTexture = textureMaddy;
                    SetAppendageParameters(_anim, .02f, .4f, 1);
                    break;  //Shark

                case Characters.Kass:
                    UnityEngine.Object.Destroy(_appendage);
                    return; //Kass is not an actual enemy character (racoon dumpster thing)
            }

            MelonCoroutines.Start(AppendageStart(_mesh, _character));   //Start appendage routine for a fresh new appendage
        }


        
        /// <summary>
        /// Set Placement,  Scale  and overall look.
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
        IEnumerator AppendageStart(SkinnedMeshRenderer _mesh, CharacterControllerBase _controller)
        {
            Animator _anim = _mesh.transform.parent.GetComponent<Animator>();

            _anim.SetFloat("Breathe", UnityEngine.Random.Range(.7f, 1.2f)); //Desync cletching speed

            _anim.SetBool("Intersex", intersex);                            //Assign current settings

            _anim.SetBool("Topless", topless);                              //Assign current settings

            activeAppendages.Add(new Appendage(_mesh.transform.parent.gameObject, _controller.character, _anim));

            if (topless)                                                    
                _controller.CostumeSwitcher.SwitchVariant("Nude");      //Undress character

            switch (LevelManagerBase.Level)
            {
                case Level.SecurityOffice:
                    SecurityOfficeCharacterController _officeController = _controller.GetComponent<SecurityOfficeCharacterController>();
                    while (_mesh)                                       //Cycle while appendage eхists
                    {
                        _anim.SetFloat("Enlarge", .5f);
                        MelonLogger.Msg(_controller.character + " @: " + _officeController.currentPosition);
                        yield return new WaitForSeconds(1);
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
        public static void FunTime(Characters _char, bool _fun)
        {
            if (_char == Characters.Everyone)
                foreach (Appendage _appendage in activeAppendages)
                    _appendage._Animator.SetBool("FunTime", _fun);                  //Set _fun to every character on a level
            else
            {
                int index = activeAppendages.FindIndex(x => x._Character == _char);     //Honesstly, idk, just copy/pasted from SO, and it works :p
                if (index >= 0)
                    activeAppendages[index]._Animator.SetBool("FunTime", _fun);     //Set _fun to a speciffic character's appendage
            }
        }




        #endregion




        #region Settings




        /// <summary>
        /// Creates a generic toggle in the Adult section of Settings.
        /// </summary>
        /// <param name="_title">Title teхt for toggle in settings</param>
        /// <param name="_void">Toggle action</param>
        /// <param name="_loadState">Sets toggles' initial state on spawn</param>
        void InstantiateToggleInSettings(string _title, Action<bool> _void, bool _loadState)
        {
            GameObject _toggle = UnityEngine.Object.Instantiate(GameObject.Find("Radio - H Content"), adultSection);    //Get and spawn a generic toggle

            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontEvent>());             //Remove translation junk
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontMaterialEvent>());     //Remove translation junk
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeStringEvent>());              //Remove translation junk

            _toggle.GetComponentInChildren<Il2CppTMPro.TextMeshProUGUI>().text = _title;                    //Set toggle title teхt
            _toggle.GetComponent<UIRadio>().SetRadio(_loadState, false);                                    //Set initial state for toggle
            _toggle.GetComponent<UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(_void));  //Assign listener to a state change
        }



        void SpawnCustomSettingItems()
        {
            if (adultSection)       //Check if container is assigned
                return;         //Don't do anything if it does

            GameObject _radio = GameObject.Find("Radio - H Content");                   //Find a toogle in Adult section and use it as a base for mine
            adultSection = _radio.transform.parent.GetComponent<RectTransform>();       //Get and assign a container in Settings that stores all the Adult stuff
            InstantiateToggleInSettings("Intersex Mode", AddIntersexToggleListener, intersex);  //Spawn Interseх toggle
            InstantiateToggleInSettings("Topless Mode", AddToplessToggleListener, topless);     //Spawn Topless toggle
        }



        void AddSettingsButtonListener()
        {
            if (optionsButton)      //Check if options button is assigned
                return;         //Don't do anything if it does

            optionsButton = GameObject.Find("Button - Settings").GetComponent<Button>();        //Get and assign a Settings button from the menu
            optionsButton.onClick.AddListener(new Action(() => { SpawnCustomSettingItems(); }));//Add listener to button pressed
        }



        void AddIntersexToggleListener(bool _isOn)
        {
            PlayerPrefs.SetString("Intersex", _isOn.ToString());            //Save state of toggle

            intersex = (PlayerPrefs.GetString("Intersex") == "True");       //Set Intersex global bool

            foreach (Appendage _appendage in activeAppendages)
                _appendage._Animator.SetBool("Intersex", _isOn);            //Apply it to appendages
        }


        void AddToplessToggleListener(bool _isOn)
        {
            PlayerPrefs.SetString("Topless", _isOn.ToString());             //Save state of toggle

            topless = (PlayerPrefs.GetString("Topless") == "True");         //Set Topless global bool

            foreach (Appendage _appendage in activeAppendages)
                _appendage._Animator.SetBool("Topless", _isOn);             //Apply it to appendages

            if (topless)                                                    
                foreach (CharacterControllerBase item in activeCharacters) 
                    item.CostumeSwitcher.SwitchVariant("Nude");             //Undress everyone
            else
                foreach (CharacterControllerBase item in activeCharacters)
                    item.CostumeSwitcher.SwitchVariant("Clothed");          //Everyone, put the clothes on!
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
            {
                FunTime(__instance.character, true);                                //Start the Act
            }
        }


        /// <summary>
        /// Add custom code to Funttime scene End
        /// "BeforeTimeSkip" is being called after the Fun scene's fade in
        /// </summary>
        [HarmonyPatch(typeof(NightclubCharacterController), "BeforeTimeSkip")]
        private static class ActEnd   //Function nickname for myself, so i won't forget what it's for
        {
            public static void Prefix(ref NightclubCharacterController __instance)  //Call stuff before original script's events accure
            {
                if (FutaMod.ModMain.topless)                                        //Check if Topless toggle is on
                    __instance.CostumeSwitcher.SwitchVariant("Nude");           //Undress the character

                FunTime(__instance.character, false);                               //End the Act
            }
        }




        #endregion




        /// <summary>
        /// Pause check
        /// </summary>
        public override void OnLateUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Tab))
            {
                if (Time.timeScale == 0)        //Workaround because I haven't figured out IN HEAT's signals in PubSub
                    AddSettingsButtonListener();
            }

            //DBG
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                MelonLogger.Msg("TP " + activeCharacters.Length + " characters");
                byte id = 0;
                foreach (CharacterControllerBase _obj in activeCharacters)
                {
                    _obj.transform.rotation = Quaternion.identity;
                    _obj.transform.position = Camera.main.transform.position + Camera.main.transform.right * (id - 2) + Camera.main.transform.forward * 3 + Camera.main.transform.up * -.6f;
                    id++;
                }
            }

            //DBG
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                    item.Animator.SetBool("SexCA", true);
            }


        } 
    }

}
