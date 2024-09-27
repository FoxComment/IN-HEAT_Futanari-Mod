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
        public string addressField = "InHeatFutaMod.funny";

        private const string textureSammyFile = "Assets/Futa/Textures/SammyV1.png";
        private const string textureMaddyFile = "Assets/Futa/Textures/MaddyV1.png";
        private const string texturePoppiFile = "Assets/Futa/Textures/PoppiV1.png";
        private const string textureAriFile = "Assets/Futa/Textures/AriV1.png";
        private const string textureNileFile = "Assets/Futa/Textures/NileV1.png";

        private const string spriteHContentFile = "Assets/Futa/Textures/IntersexButtonBG.png";

        private const string appendageOnePrefabFile = "Assets/Futa/Prefs/AppengadeOne.prefab";

        Texture2D textureSammy;
        Texture2D textureMaddy;
        Texture2D texturePoppi;
        Texture2D textureAri;
        Texture2D textureNile;

        Sprite spriteHContent;

        CharacterControllerBase[] activeCharacters;

        GameObject prefabAppendageOne;

        Material defaultMaterial;
        Shader defaultShader;

        //public static List<GameObject> activeAppendages= new List<GameObject>(0);

        RectTransform adultSection;

        Button optionsButton;

        bool DEBUG = true;

        TimeCharacterControllerBase dd;

        public static bool topless { get; private set; } = false; 

        public bool intersex { get; private set; } = false;


        public override void OnInitializeMelon() => LoadAssetBundle();
        void FetchCharacters() => MelonCoroutines.Start(fetchCharacters());

        public static List<Appendage> activeAppendagess;

        [System.Serializable]
        public class Appendage
        {
            public GameObject _GameObject;
            public Characters _Character;
            public Animator _Animator;

            //Constructor (not necessary, but helpful)
            public Appendage(GameObject _gameobject, Characters _character, Animator _animator)
            {
                _GameObject = _gameobject;
                _Character = _character;
                _Animator = _animator;
            }
        }

        IEnumerator fetchCharacters()
        {
            //Give this thing a moment 2 think cus i hab no idea on how to recieve Signals with Il2cpp
            yield return new WaitForSeconds(.5f);

            activeCharacters = GameObject.FindObjectsOfType<CharacterControllerBase>();

            yield return new WaitForSeconds(.1f);
            SpawnAppendagesForActiveChatacters();
        }



        void SpawnAppendagesForActiveChatacters()
        {
            foreach (CharacterControllerBase _character in activeCharacters)
            {
                var children = _character.Animator.transform.GetChild(0).GetComponentsInChildren<Transform>();
                foreach (var child in children)
                    if (child.name == "DEF-spine")
                    {
                        SpawnAppendage(_character, child.transform);
                        break;
                    }
            }
        }


        [HarmonyPatch(typeof(NightclubCharacterController), "OnAttackTransitionEnd")]
        private static class ActStart
        {
            public static void Prefix(ref NightclubCharacterController __instance)
            {

                //foreach(GameObject child in FutaMod.ModMain.activeAppendages)
                //  child.GetComponent<Animator>().SetBool("FunTime", true);

                FunTime(__instance.character, true);


                MelonLogger.Msg("PSSSS  "+ __instance.character+" ");

            }
        }

        [HarmonyPatch(typeof(NightclubCharacterController), "BeforeTimeSkip")]
        private static class ActEnd
        {
            public static void Prefix(ref NightclubCharacterController __instance)
            {

                if (FutaMod.ModMain.topless)
                    __instance.CostumeSwitcher.SwitchVariant("Nude");

                FunTime(__instance.character, false);


                MelonLogger.Msg("PSSSS222");

            }
        }

        IEnumerator AppendageAlive(SkinnedMeshRenderer _mesh, CharacterControllerBase _controller)
        {
            Animator _anim = _mesh.transform.parent.GetComponent<Animator>();

            _anim.SetFloat("Breathe", UnityEngine.Random.Range(.7f,1.2f));  //Desync cletching speed

            _anim.SetBool("Intersex", intersex);

            _anim.SetBool("Topless", topless);

            activeAppendagess.Add(new Appendage(_mesh.transform.parent.gameObject, _controller.character, _anim));

            if (topless)
                _controller.CostumeSwitcher.SwitchVariant("Nude");

            switch (LevelManagerBase.Level)
            {
                case Level.SecurityOffice:
                    SecurityOfficeCharacterController _officeController = _controller.GetComponent<SecurityOfficeCharacterController>(); 
                    while (_mesh)
                    {
                        _anim.SetFloat("Enlarge", .5f);
                        MelonLogger.Msg( _controller.character + " @: " + _officeController.currentPosition);
                        yield return new WaitForSeconds(1);
                    }
                    break;

                case Level.Nightclub:

                    NightclubCharacterController _clubController = _controller.GetComponent<NightclubCharacterController>();
                    while (_mesh)
                    {
                        _anim.SetFloat("Enlarge", _clubController._excitement._excitement);
                        _anim.SetBool("Leak", _clubController.IsExcited);

                        yield return new WaitForSeconds(.1f);
                    }
                    break;
            }

        }

        private void SpawnAppendage(CharacterControllerBase _character, Transform _hipBone)
        {
            MelonLogger.Msg("Added for: " + _hipBone.name);

            GameObject _appendage = UnityEngine.Object.Instantiate(prefabAppendageOne, _hipBone);
            _appendage.tag = _character.character.ToString();
            SkinnedMeshRenderer _mesh = _appendage.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
            Animator _anim = _appendage.GetComponent<Animator>();

            _mesh.material = defaultMaterial;
            _mesh.material.shader = defaultShader;
            _mesh.material.SetFloat("_Smoothness", 0);
            _mesh.material.SetFloat("_EnvironmentReflections", 0);

            switch (_character.character) 
            {
                case Characters.Ari:
                    _mesh.material.mainTexture = textureAri;
                    SetAppendageParameters(_anim, .103f, .49f, 0, .02f);
                    break;

                case Characters.Sammy:
                    _mesh.material.mainTexture = textureSammy;
                    SetAppendageParameters(_anim, .065f, .68f,.3f,.16f);
                    break;

                case Characters.Poppi:
                    _mesh.material.mainTexture = texturePoppi;
                    SetAppendageParameters(_anim, .071f, .66f, 0, .1f);
                    break;

                case Characters.Nile:
                    _mesh.material.mainTexture = textureNile;
                    SetAppendageParameters(_anim, .091f, .55f, 0, .45f);
                    break;

                case Characters.Maddie:
                    _mesh.material.mainTexture = textureMaddy;
                    SetAppendageParameters(_anim, .13f, .7f, .7f, .35f);
                    break;
                
                case Characters.Misty:
                    _mesh.material.mainTexture = textureMaddy;
                    SetAppendageParameters(_anim, .02f, .4f, 1);
                    break;

                case Characters.Kass:
                    UnityEngine.Object.Destroy(_appendage);
                    return;
            }

            MelonCoroutines.Start(AppendageAlive(_mesh, _character));

            MelonLogger.Msg(_character.Character + " is a Futa!");

            //   activeAppendages.Add(_appendage);
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
            _anim.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _anim.transform.localPosition = Vector3.forward * _offest;
            _anim.transform.localScale = Vector3.one * _scale;
            _anim.SetFloat("Fertility", _fertility);
            _anim.SetFloat("Thickness", _diameter);
        }




        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            FetchDefaultAssets();

            activeCharacters = new CharacterControllerBase[0];
            //activeAppendages = new List<GameObject>(0);
            activeAppendagess = new List<Appendage>(0);
            activeAppendagess.Clear();

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
            else
            if (GameObject.FindObjectOfType<AdultSpriteSwitcher>() != null)
            {
                AdultSpriteSwitcher _ad = GameObject.FindObjectOfType<AdultSpriteSwitcher>();
                _ad._nsfwSprite = spriteHContent;
                _ad.UpdateSprite(Il2CppMonsterBox.Systems.Saving.SaveManager.LoadSettings().HContent);

                SettingsButtonListener();
            }
        }
        



        void FetchDefaultAssets()
        {
            intersex = (PlayerPrefs.GetString("Intersex") == "True");
            topless = (PlayerPrefs.GetString("Topless") == "True");

            if (defaultShader != null)
                return;

            if (GameObject.FindObjectOfType<SkinnedMeshRenderer>() == null)
                return;

            defaultShader = GameObject.FindObjectOfType<SkinnedMeshRenderer>().material.shader;
            defaultShader.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            MelonLogger.Msg(defaultShader.name);
        }




        void LoadAssetBundle()
        {
            MemoryStream memoryStream;

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

            prefabAppendageOne = _bundle.LoadAsset_Internal(appendageOnePrefabFile, Il2CppType.Of<GameObject>()).Cast<GameObject>();
           
            texturePoppi = _bundle.LoadAsset_Internal(texturePoppiFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureNile = _bundle.LoadAsset_Internal(textureNileFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureSammy = _bundle.LoadAsset_Internal(textureSammyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAri = _bundle.LoadAsset_Internal(textureAriFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureMaddy = _bundle.LoadAsset_Internal(textureMaddyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();

            Texture2D _spriteTemp = _bundle.LoadAsset_Internal(spriteHContentFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            spriteHContent = Sprite.Create(_spriteTemp, new Rect(0, 0, _spriteTemp.width, _spriteTemp.height), Vector2.one * .5f, 100);

            prefabAppendageOne.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            texturePoppi.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureNile.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureSammy.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAri.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureMaddy.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            spriteHContent.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }



        void SpawnItemInSettings(string _title, Action<bool> _void, bool _loadState)
        {
            GameObject _toggle = UnityEngine.Object.Instantiate(GameObject.Find("Radio - H Content"), adultSection);

            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontEvent>());
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeTMPFontMaterialEvent>());
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeStringEvent>());

            _toggle.GetComponentInChildren<Il2CppTMPro.TextMeshProUGUI>().text = _title;
            _toggle.GetComponent<UIRadio>().SetRadio(_loadState, false);
            _toggle.GetComponent<UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityAction<bool>>(_void));



            //_radio.GetComponent<Il2CppMonsterBox.Systems.UI.Elements.UIScrollElement>().;
            //_radio.GetComponent<Il2CppMonsterBox.Runtime.UI.Settings.TooltipElement>().;
            //UnityEngine.Component[] _comps = _radio.transform.GetComponents(Il2CppType.Of<UnityEngine.Component>());
            //foreach (UnityEngine.Component item in _comps)
            //{
            //    MelonLogger.Msg("Comps1: " + item.ToString());
            //    MelonLogger.Msg("Comps2: " + item.ObjectClass);
            //    MelonLogger.Msg("Comps3: " + item.name);
            //    MelonLogger.Msg("Comps4: " + item + "\n");
            //}
        }




        void SpawnCustomSettingItems()
        {
            if (adultSection)
                return;

            GameObject _radio = GameObject.Find("Radio - H Content");
            adultSection = _radio.transform.parent.GetComponent<RectTransform>();
            SpawnItemInSettings("Intersex Mode", FutaButtonListener, intersex);
            SpawnItemInSettings("Topless Mode", ToplessButtonListener, topless);
            MelonLogger.Msg(_radio.transform.parent.name);;
        }
        void FutaButtonListener(bool _isOn)
        {
            PlayerPrefs.SetString("Intersex", _isOn.ToString());

            intersex = (PlayerPrefs.GetString("Intersex") == "True");

            foreach (Appendage _appendage in activeAppendagess)
                _appendage._Animator.SetBool("Intersex", _isOn);
        }

        public static void FunTime(Characters _char, bool _fun)
        {
            if (_char == Characters.Everyone) 
            {
                foreach (Appendage _appendage in activeAppendagess)
                    _appendage._Animator.SetBool("FunTime", _fun);
            }
            else
            {
                int index = activeAppendagess.FindIndex(x => x._Character == _char);
                if (index >= 0)
                    activeAppendagess[index]._Animator.SetBool("FunTime", _fun);
            }
        }

        void ToplessButtonListener(bool _isOn)
        {
            PlayerPrefs.SetString("Topless", _isOn.ToString());

            topless = (PlayerPrefs.GetString("Topless") == "True");

            foreach (Appendage _appendage in activeAppendagess)
                _appendage._Animator.SetBool("Topless", _isOn);

            if (topless)
                foreach (CharacterControllerBase item in activeCharacters)
                    item.CostumeSwitcher.SwitchVariant("Nude");

            else
                foreach (CharacterControllerBase item in activeCharacters)
                    item.CostumeSwitcher.SwitchVariant("Clothed");
        }




        void SettingsButtonListener()
        {
            if (optionsButton)
                return;

            optionsButton = GameObject.Find("Button - Settings").GetComponent<Button>();
            optionsButton.onClick.AddListener(new Action(() => { SpawnCustomSettingItems(); }));
        }
        public override void OnLateUpdate()
        {
            
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.LeftAlt))
            {
                
                MelonLogger.Msg("Paused: " + (Time.timeScale == 0));

                if (Time.timeScale != 0)
                    return;
                SettingsButtonListener();

                //MelonLogger.Msg("Active Menu Man: " + (GameObject.FindObjectOfType<Il2CppMonsterBox.Systems.UI.UIManagerBase>()) != null);

                //MelonLogger.Msg("Focused UI: "+GameObject.FindObjectOfType<Il2CppMonsterBox.Systems.UI.UIManagerBase>().ActivePanel.name);
      
                //MelonLogger.Msg("AD Section prent: " + GameObject.Find("Button - Settings"));
            }


            if (Input.GetKeyDown(KeyCode.J))
            {
                RectTransform[] _itm = GameObject.FindObjectsOfType<RectTransform>();
                foreach (RectTransform _itmNam in _itm)
                    MelonLogger.Msg(_itmNam.gameObject.name);
            }
            //Button - Settings


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
                


              //  ButtonHintManager fsa;
              //  fsa.
               // MelonLogger.Msg("Ad section found? " + (GameObject.FindObjectOfType<SettingsUIManager>(true) != null));
            }


            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", true);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", true);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", false);





                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", true);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", true);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", true);
                    item.Animator.SetBool("SexSFW", false);
                }
            }


            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                foreach (CharacterControllerBase item in activeCharacters) 
                {
                    item.Animator.SetBool("IdleA", false);
                    item.Animator.SetBool("SexA", false);
                    item.Animator.SetBool("SexCA", false);
                    item.Animator.SetBool("SexCB", false);
                    item.Animator.SetBool("SexCC", false);
                    item.Animator.SetBool("SexCD", false);
                    item.Animator.SetBool("SexSFW", true); 
                }
            }



            if (Input.GetKeyDown(KeyCode.C))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                {
                    MelonLogger.Msg(item.name+" :");
                    foreach (VariantConfig _va in item.CostumeSwitcher.variantConfigs)
                    MelonLogger.Msg(_va.variantName);
                    MelonLogger.Msg("");
                }
            }
        } 
    }





    /*
    void OnEnableBehaviour()
    {
    //SIGNAL TESTING
    //SIGNAL TESTING
    //SIGNAL TESTING
    //SIGNAL TESTING
    //SIGNAL TESTING
    //SIGNAL TESTING
    //SIGNAL TESTING


        Il2CppMonsterBox.Systems.Tools.PubSub.SignalEvent ds = new Il2CppMonsterBox.Systems.Tools.PubSub.SignalEvent("Adult",false);
        adultSwitch.OnSignal(ds);
        Bool sd = ds.GetValue<bool>();


    //SIGNAL TESTING
    //SIGNAL TESTING
    //SIGNAL TESTING
    //SIGNAL TESTING
    //SIGNAL TESTING
    }
    void Idk(bool _nsfw, long _paramID)
    {

    }
    void IDK()
    {

    }*/

    //MelonLogger.Msg("Font: "+(Resources.GetBuiltinResource(Il2CppType.Of<Font>(), "LegacyRuntime.ttf") as Font!=null));
    //  Font ds = Resources.GetBuiltinResource(Il2CppType.Of<Font>(), "LegacyRuntime.ttf") as Font;
    //MelonLogger.Msg(ds.name);

    //evemt.delegates.Add(myDelegate);
    //cameraTRA = Camera.main.transform;
    //MelonLogger.Msg("Font: "+(Resources.GetBuiltinResource(Il2CppType.Of<Font>(), "LegacyRuntime.ttf") as Font!=null));
    //  Font ds = Resources.GetBuiltinResource(Il2CppType.Of<Font>(), "LegacyRuntime.ttf") as Font;
    //MelonLogger.Msg(ds.name);
    // Il2CppMonsterBox.Systems.Saving.Data.SettingsData;
    //Il2CppMonsterBox.Runtime.UI.Settings.SettingsUIManager d;
    //d;
    //PlayerPrefs.SetString("FUTANARI THIGN", "FALACE");
    //Il2CppMonsterBox.Systems.Tools.PubSub.SignalEvent ds;
    //Il2CppMonsterBox.Systems.Tools.PubSub.Subscriber dss;
    //AdultSwitcher d;
    //d.Subscriber.add_OnSignal(evemt);
    //dss.OnSignal;
    //MelonLogger.Msg(ds.GetValue());
    //MelonLogger.Msg(ds.GetValue());
    //dss.Subscribe("ds");
    //dss.on
    //  Font ase = Resources.GetBuiltinResource(Il2CppType.Of<Font>()).Cast<Font>("LegacyRuntime"));


}
