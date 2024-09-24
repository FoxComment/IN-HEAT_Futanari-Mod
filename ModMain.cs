using MelonLoader;
using UnityEngine;
using FutaMod;
using Il2CppMonsterBox;
using UnityEngine.Rendering.Universal;
using Il2CppParadoxNotion;
using Harmony;
using HarmonyLib;
using System.Collections;
using Il2CppMono.Unity;
using Il2CppNodeCanvas.Tasks.Actions;
using UnityEngine.UI;
using Il2CppInterop.Runtime;
using Il2CppTMPro;
using UnityEngine.TextCore.Text;
using I18N.Common;
using Il2Cpp;
using Il2CppMonsterBox.Runtime.Core.Gameplay;
using Il2CppMonsterBox.Systems.Tools.Miscellaneous;
using Il2CppMonsterBox.Systems;
using Il2CppMonsterBox.Systems.Tools;
using Il2CppMonsterBox.Systems.Tools.Accessors;
using Il2CppMonsterBox.Systems.Tools.Interfaces;
using Il2CppMonsterBox.Systems.Tools.PubSub;
using Il2CppMonsterBox.Systems.Tools.PubSub.Interfaces;
using Il2CppMonsterBox.Systems.Camera;
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
using Il2CppRewired.Platforms;
using UnityEngine.Events;
using Il2CppSystem.Runtime.InteropServices;
using Il2CppMonsterBox.Runtime.Gameplay.Interactables;
using Il2CppSystem.ComponentModel;
using Il2CppMonsterBox.Systems.UI.Elements;
using UnityEngine.Localization.Components;
using MonsterBox.Runtime.Extensions._Localization;
using MonsterBox.Systems.UI.Elements;
using Il2CppMonsterBox.Runtime.Extensions._NodeCanvas.Tasks.Game.Customer;
[assembly: MelonInfo(typeof(ModMain), "FutanariMod", "10.10.2024", "FoxComment", "https://github.com/foxcomment/inheat_futamod")]
[assembly: MelonGame("MonsterBox", "IN HEAT")]
namespace FutaMod
{

    public class ModMain : MelonMod
    {
        GameObject[] goNames = new GameObject[0];
        GameObject[] listObjectDirectoryActive = new GameObject[0];

        string objectList = " asdvdfvbsed df s d sdfg sedfdfgedfsdffvgds vbdf ";
        public string addressField = "InHeatFutaMod.funny.assetbundle";

        private const string textureAppendageSammyFile = "Assets/Futa/Textures/SammyV1.png";
        private const string textureAppendageMaddyFile = "Assets/Futa/Textures/MaddyV1.png";
        private const string textureAppendagePoppiFile = "Assets/Futa/Textures/PoppiV1.png";
        private const string textureAppendageAriFile = "Assets/Futa/Textures/AriV1.png";
        private const string textureAppendageNileFile = "Assets/Futa/Textures/NileV1.png";

        private const string spriteHContentFile = "Assets/Futa/Textures/IntersexButtonBG.png";

        private const string appendageOnePrefabFile = "Assets/Futa/Prefs/AppengadeOne.prefab";

        private const string prefabIntersexSettingsItemFile = "Assets/Futa/Prefs/SettingIntersex.prefab";

        Texture2D textureAppendageSammy;
        Texture2D textureAppendageMaddy;
        Texture2D textureAppendagePoppi;
        Texture2D textureAppendageAri;
        Texture2D textureAppendageNile;

        Sprite spriteHContent;

        CharacterControllerBase[] activeCharacters;

        GameObject prefabAppendageOne;


        RectTransform prefabIntersexSettingsItem;

        Material defaultMaterial;
        Shader defaultShader;

        List<GameObject> activeAppendages = new List<GameObject>(0);

        RectTransform adultSection;

        Button optionsButton;




        bool DEBUG = true;
        public override void OnInitializeMelon()
        {
            LoadAssetBundle();            
            
        }




        IEnumerator FetchCharacters()
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


        IEnumerator AppendageAlive(SkinnedMeshRenderer _mesh, CharacterControllerBase _controller)
        {
            if (PlayerPrefs.GetString("Topless") == "True")
            {
                _controller.CostumeSwitcher.defaultVariant = "Nude";

                _controller.CostumeSwitcher.SwitchVariant("Nude");
            }

            Animator _anim = _mesh.transform.parent.GetComponent<Animator>();
            MelonLogger.Msg("Cuck Created for " + _controller.character + " Deciding Stuff");

            _anim.SetFloat("Breathe", UnityEngine.Random.Range(.7f,1.2f));  //Desync cletching speed

            switch (Il2CppMonsterBox.Runtime.Gameplay.Level.LevelManagerBase.Level)
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

            _mesh.enabled = (PlayerPrefs.GetString("Intersex") == "True");
        }

        private void SpawnAppendage(CharacterControllerBase _character, Transform _hipBone)
        {
            MelonLogger.Msg("Added for: " + _hipBone.name);

            GameObject _appendage = UnityEngine.Object.Instantiate(prefabAppendageOne, _hipBone);
            SkinnedMeshRenderer _mesh = _appendage.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
            Animator _anim = _appendage.GetComponent<Animator>();

            _mesh.material = defaultMaterial;
            _mesh.material.shader = defaultShader;
            _mesh.material.SetFloat("_Smoothness", 0);
            _mesh.material.SetFloat("_EnvironmentReflections", 0);

            switch (_character.character) 
            {
                case Characters.Ari:
                    _mesh.material.mainTexture = textureAppendageAri;
                    SetAppendageParameters(_anim, .103f, .49f, 0, .02f);
                    break;

                case Characters.Sammy:
                    _mesh.material.mainTexture = textureAppendageSammy;
                    SetAppendageParameters(_anim, .068f, .68f,.3f,.2f);
                    break;

                case Characters.Poppi:
                    _mesh.material.mainTexture = textureAppendagePoppi;
                    SetAppendageParameters(_anim, .071f, .66f, 0, .1f);
                    break;

                case Characters.Nile:
                    _mesh.material.mainTexture = textureAppendageNile;
                    SetAppendageParameters(_anim, .091f, .55f, 0, .55f);
                    break;

                case Characters.Maddie:
                    _mesh.material.mainTexture = textureAppendageMaddy;
                    SetAppendageParameters(_anim, .13f, .7f, .7f, .35f);
                    break;
                
                case Characters.Misty:
                    _mesh.material.mainTexture = textureAppendageMaddy;
                    SetAppendageParameters(_anim, .02f, .4f, 1);
                    break;

                case Characters.Kass:
                    UnityEngine.Object.Destroy(_appendage);
                    return;
            }

            MelonCoroutines.Start(AppendageAlive(_mesh, _character));

            MelonLogger.Msg(_character +" is a Futa!");

            activeAppendages.Add(_appendage);
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
            //AddIntersexToggleToSettings();

            activeCharacters = new CharacterControllerBase[0];

            if (Il2CppMonsterBox.Runtime.Gameplay.Level.LevelManagerBase.Exists)
                switch (Il2CppMonsterBox.Runtime.Gameplay.Level.LevelManagerBase.Level)
                {
                    case Level.SecurityOffice:
                        MelonLogger.Msg("Office Level");
                        MelonCoroutines.Start(FetchCharacters());
                        break;
                    case Level.Nightclub:
                        MelonLogger.Msg("NightClub Level");
                        MelonCoroutines.Start(FetchCharacters());
                        break;
                }
            else
            if (GameObject.FindObjectOfType<AdultSpriteSwitcher>() != null)
            {
                AdultSpriteSwitcher _ad = GameObject.FindObjectOfType<AdultSpriteSwitcher>();
                _ad._nsfwSprite = spriteHContent;
                _ad.UpdateSprite(Il2CppMonsterBox.Systems.Saving.SaveManager.LoadSettings().HContent);
            }
        }
        

        void FetchDefaultAssets()
        {
            //cameraTRA = Camera.main.transform;


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
           
           // prefabIntersexSettingsItem = _bundle.LoadAsset_Internal(prefabIntersexSettingsItemFile, Il2CppType.Of<RectTransform>()).Cast<RectTransform>();

            textureAppendagePoppi = _bundle.LoadAsset_Internal(textureAppendagePoppiFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAppendageNile = _bundle.LoadAsset_Internal(textureAppendageNileFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAppendageSammy = _bundle.LoadAsset_Internal(textureAppendageSammyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAppendageAri = _bundle.LoadAsset_Internal(textureAppendageAriFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            textureAppendageMaddy = _bundle.LoadAsset_Internal(textureAppendageMaddyFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();

            Texture2D _spriteTemp = _bundle.LoadAsset_Internal(spriteHContentFile, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            spriteHContent = Sprite.Create(_spriteTemp, new Rect(0, 0, _spriteTemp.width, _spriteTemp.height), Vector2.one * .5f, 100);

            prefabAppendageOne.hideFlags |= HideFlags.DontUnloadUnusedAsset;

           // prefabIntersexSettingsItem.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            textureAppendagePoppi.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAppendageNile.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAppendageSammy.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAppendageAri.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            textureAppendageMaddy.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            spriteHContent.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        }

        //public override void Signal(bool trigger, string signal)
        //{ 
        //}

        void SpawnItemInSettings(string _title, Action<bool> _void)
        {
            GameObject _toggle = UnityEngine.Object.Instantiate(GameObject.Find("Radio - H Content"), adultSection);

            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<Il2CppMonsterBox.Runtime.Extensions._Localization.LocalizeTMPFontEvent>());
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<Il2CppMonsterBox.Runtime.Extensions._Localization.LocalizeTMPFontMaterialEvent>());
            UnityEngine.Object.Destroy(_toggle.GetComponentInChildren<LocalizeStringEvent>());

            _toggle.GetComponentInChildren<Il2CppTMPro.TextMeshProUGUI>().text = _title;

            _toggle.GetComponent<Il2CppMonsterBox.Systems.UI.Elements.UIRadio>().onRadioChanged.AddListener(DelegateSupport.ConvertDelegate<UnityEngine.Events.UnityAction<bool>>(_void));



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
            SpawnItemInSettings("Intersex Mode", FutaButtonListener);
            SpawnItemInSettings("Topless Mode", ToplessButtonListener);
            MelonLogger.Msg(_radio.transform.parent.name);;
        }
        void FutaButtonListener(bool _isOn)
        {
            PlayerPrefs.SetString("Intersex", _isOn.ToString());

            foreach (GameObject _appendage in activeAppendages)
                _appendage.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = (PlayerPrefs.GetString("Intersex") == "True");
        }


        void ToplessButtonListener(bool _isOn)
        {
            PlayerPrefs.SetString("Topless", _isOn.ToString());

            foreach (GameObject _appendage in activeAppendages)
                _appendage.GetComponent<Animator>().SetBool("Topless", (PlayerPrefs.GetString("Topless") == "True"));

            if (PlayerPrefs.GetString("Topless") == "True")
                foreach (CharacterControllerBase item in activeCharacters)
                {
                    item.CostumeSwitcher.SwitchVariant("Nude");
                    item.CostumeSwitcher.defaultVariant = "Nude";
                }

            else
                foreach (CharacterControllerBase item in activeCharacters)
                {
                    item.CostumeSwitcher.SwitchVariant("Clothed");
                    item.CostumeSwitcher.defaultVariant = "Clothed";
                }
        }




        void SettingsButtonListener()
        {
            if (!optionsButton)
            {
                optionsButton = GameObject.Find("Button - Settings").GetComponent<Button>();
                optionsButton.onClick.AddListener(new Action(() => { SpawnCustomSettingItems(); }));
            }
        }
        public override void OnLateUpdate()
        {
            
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.LeftAlt))
            {
                
                MelonLogger.Msg("Paused: " + (Time.timeScale == 0));


                if (Time.timeScale != 0)
                    return;
                SettingsButtonListener();

              //  MelonLogger.Msg("Active Menu Man: " + (GameObject.FindObjectOfType<Il2CppMonsterBox.Systems.UI.UIManagerBase>()) != null);

               // MelonLogger.Msg("Focused UI: "+GameObject.FindObjectOfType<Il2CppMonsterBox.Systems.UI.UIManagerBase>().ActivePanel.name);
                //MelonLogger.Msg("Kids amt: "+GameObject.FindObjectOfType<Il2CppMonsterBox.Systems.UI.UIManagerBase>().ActivePanel.transform.childCount);
                //GameObject _adultSectionTRA = GameObject.FindObjectOfType<SettingsUIManager>().gameObject;
                //MelonLogger.Msg(_adultSectionTRA.gameObject.name);
                //MelonLogger.Msg(_adultSectionTRA.transform.GetChildCount());
                //
      //          Il2CppMonsterBox.Systems.UI.UIManagerBase _TMP2 = GameObject.FindObjectOfType<Il2CppMonsterBox.Systems.UI.UIManagerBase>();
      //           for (int i = 0; i < _TMP2.ActivePanel.transform.childCount-1; i++)
      //              MelonLogger.Msg(_TMP2.ActivePanel.transform.GetChild(i).gameObject.name);

                //MelonLogger.Msg("AD Section prent: " + GameObject.Find("Button - Settings"));

                //MelonLogger.Msg("\n\n");
                /*
                Transform _TMP3 = GameObject.FindObjectOfType<Il2CppMonsterBox.Systems.UI.UIManagerBase>().transform.GetChild(0);
                for (int i = 0; i < _TMP3.childCount - 1; i++)
                    MelonLogger.Msg(_TMP3.GetChild(i).gameObject.name);

                Transform _TMP4 = GameObject.FindObjectOfType<Il2CppMonsterBox.Runtime.UI.Settings.SettingsAdultSection>().transform.GetChild(0);
                for (int i = 0; i < _TMP4.childCount - 1; i++)
                    MelonLogger.Msg(_TMP4.GetChild(i).gameObject.name);
            */}


            if (Input.GetKeyDown(KeyCode.J))
            {
                RectTransform[] _itm = GameObject.FindObjectsOfType<RectTransform>();
                foreach (RectTransform _itmNam in _itm)
                    MelonLogger.Msg(_itmNam.gameObject.name);
            }
            //Radio - H Content
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
                
              //  Font ase = Resources.GetBuiltinResource(Il2CppType.Of<Font>()).Cast<Font>("LegacyRuntime"));


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


            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                    item.CostumeSwitcher.SwitchVariant("Nude");
            }


            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                    item.CostumeSwitcher.SwitchVariant("Hypnotized");
            }


            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                foreach (CharacterControllerBase item in activeCharacters)
                    item.CostumeSwitcher.SwitchVariant("Clothed");
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


}
