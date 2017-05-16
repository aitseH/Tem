using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Tem;

namespace Tem
{
    public class CollectibleEditorWindow : TemEditorWindow
    {

        private static CollectibleEditorWindow window;

        public static void Init()
        {

            window = (CollectibleEditorWindow)EditorWindow.GetWindow(typeof(CollectibleEditorWindow), false, "Collectible Editor");
            window.minSize = new Vector2(400, 300);

            LoadDB();

            InitLabel();

            window.SetupCallback();

            window.OnSelectionChange();

        }



        private static string[] collectTypeLabel;
        private static string[] collectTypeTooltip;

        private static string[] weaponTypeLabel;
        private static string[] weaponTypeTooltip;

        private static void InitLabel()
        {
            int enumLength = Enum.GetValues(typeof(_CollectType)).Length;
            collectTypeLabel = new string[enumLength];
            collectTypeTooltip = new string[enumLength];

            for (int n = 0; n < enumLength; n++)
            {
                collectTypeLabel[n] = ((_CollectType)n).ToString();

                if ((_CollectType)n == _CollectType.Self) collectTypeTooltip[n] = "affects the player only";
            }
        }


        public void SetupCallback()
        {
            selectCallback = this.SelectItem;
            shiftItemUpCallback = this.ShiftItemUp;
            shiftItemDownCallback = this.ShiftItemDown;
            deleteItemCallback = this.DeleteItem;
        }


        public override void Awake()
        {
            base.Awake();
        }

        void OnEnable()
        {
            OnSelectionChange();
        }

        public override void Update()
        {
            base.Update();
        }


        public override bool OnGUI()
        {
            if (!base.OnGUI()) return true;

            if (window == null) Init();

            List<Collectible> collectibleList = collectibleDB.collectibleList;

            Undo.RecordObject(this, "window");
            Undo.RecordObject(collectibleDB, "collectibleDB");
            if (collectibleList.Count > 0 && selectID >= 0) Undo.RecordObject(collectibleList[selectID], "collectible");

            if (GUI.Button(new Rect(Math.Max(260, window.position.width - 120), 5, 100, 25), "Save")) SetDirtyTem();


            EditorGUI.LabelField(new Rect(5, 7, 150, 17), "Add New Collectible:");
            Collectible newCollectible = null;
            newCollectible = (Collectible)EditorGUI.ObjectField(new Rect(125, 7, 140, 17), newCollectible, typeof(Collectible), false);
            if (newCollectible != null) Select(NewItem(newCollectible));


            float startX = 5; float startY = 55;

            if (minimiseList)
            {
                if (GUI.Button(new Rect(startX, startY - 20, 30, 18), ">>")) minimiseList = false;
            }
            else
            {
                if (GUI.Button(new Rect(startX, startY - 20, 30, 18), "<<")) minimiseList = true;
            }

            Vector2 v2 = DrawCollectibleList(startX, startY, collectibleList);
            startX = v2.x + 25;

            if (collectibleList.Count == 0) return true;


            Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
            Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);


            scrollPos = GUI.BeginScrollView(visibleRect, scrollPos, contentRect);

            if (srlObj.isEditingMultipleObjects)
            {
                EditorGUI.HelpBox(new Rect(startX, startY, width + spaceX, 40), "More than 1 Collectible instance is selected\nMulti-instance editing is not supported\nTry use Inspector instead", MessageType.Warning);
                startY += 55;
            }

            Collectible cltToEdit = selectedCltList.Count != 0 ? selectedCltList[0] : collectibleList[selectID];

            Undo.RecordObject(cltToEdit, "cltToEdit");

            v2 = DrawCollectibleConfigurator(startX, startY, cltToEdit);
            contentWidth = v2.x + 35;
            contentHeight = v2.y - 55;

            srlObj.ApplyModifiedProperties();

            if (selectedCltList.Count > 0 && TemEditor.IsPrefabInstance(selectedCltList[0].gameObject))
            {
                PrefabUtility.RecordPrefabInstancePropertyModifications(selectedCltList[0]);
            }

            GUI.EndScrollView();

            if (GUI.changed)
            {
                SetDirtyTem();
                for (int i = 0; i < selectedCltList.Count; i++) EditorUtility.SetDirty(selectedCltList[i]);
            }

            return true;
        }

		Vector2 DrawCollectibleConfigurator(float startX, float startY, Collectible cItem){

			return new Vector2(startX, startY+200);
		}


        protected Vector2 DrawCollectibleList(float startX, float startY, List<Collectible> collectibleList)
        {
            List<Item> list = new List<Item>();
            for (int i = 0; i < collectibleList.Count; i++)
            {
                Item item = new Item(collectibleList[i].ID, collectibleList[i].collectibleName, collectibleList[i].icon);
                list.Add(item);
            }
            return DrawList(startX, startY, window.position.width, window.position.height, list);
        }

        public static int NewItem(Collectible collectible) { return window._NewItem(collectible); }
        int _NewItem(Collectible collectible)
        {
            if (collectibleDB.collectibleList.Contains(collectible)) return selectID;

            collectible.ID = GenerateNewID(collectibleIDList);
            collectibleIDList.Add(collectible.ID);

            collectibleDB.collectibleList.Add(collectible);

            UpdateLabel_Collectible();

            return collectibleDB.collectibleList.Count - 1;
        }

        void DeleteItem()
        {
            collectibleIDList.Remove(collectibleDB.collectibleList[deleteID].ID);
            collectibleDB.collectibleList.RemoveAt(deleteID);

            UpdateLabel_Collectible();
        }
        void ShiftItemUp() { if (selectID > 0) ShiftItem(-1); }
        void ShiftItemDown() { if (selectID < collectibleDB.collectibleList.Count - 1) ShiftItem(1); }
        void ShiftItem(int dir)
        {
            Collectible collectible = collectibleDB.collectibleList[selectID];
            collectibleDB.collectibleList[selectID] = collectibleDB.collectibleList[selectID + dir];
            collectibleDB.collectibleList[selectID + dir] = collectible;
            selectID += dir;
        }
        void SelectItem()
        {
            if (collectibleDB.collectibleList.Count <= 0)
                return;
            selectID = Mathf.Clamp(selectID, 0, collectibleDB.collectibleList.Count - 1);
            Selection.activeGameObject = collectibleDB.collectibleList[selectID].gameObject;
            SerializeItemInUnitList();
        }
        private SerializedObject srlObj;
        public List<Collectible> selectedCltList = new List<Collectible>(); //unit selected in hierarchy or project-tab

        void SerializeItemInUnitList()
        {
            srlObj = null;
            if (collectibleDB != null && collectibleDB.collectibleList.Count > 0)
            {
                if (selectID < 0) selectID = 0;
                srlObj = new SerializedObject((UnityEngine.Object)collectibleDB.collectibleList[selectID]);
            }
        }


        void OnSelectionChange()
        {
            if (window == null) return;

            srlObj = null;

            selectedCltList = new List<Collectible>();

            UnityEngine.Object[] filtered = Selection.GetFiltered(typeof(Collectible), SelectionMode.Editable);
            for (int i = 0; i < filtered.Length; i++) selectedCltList.Add((Collectible)filtered[i]);

            //if no no relevent object is selected
            if (selectedCltList.Count == 0)
            {
                SelectItem();
                if (collectibleDB.collectibleList.Count > 0 && selectID >= 0)
                    UpdateObjectHierarchyList(collectibleDB.collectibleList[selectID].gameObject);
            }
            else
            {
                //only one relevent object is selected
                if (selectedCltList.Count == 1)
                {
                    //if the selected object is a prefab and match the selected item in editor, do nothing
                    if (selectID > 0 && selectedCltList[0] == collectibleDB.collectibleList[selectID])
                    {
                        UpdateObjectHierarchyList(selectedCltList[0].gameObject);
                    }
                    //if the selected object doesnt match...
                    else
                    {
                        //if the selected object existed in DB
                        if (TemEditor.ExistInDB(selectedCltList[0]))
                        {
                            window.selectID = TemEditor.GetCollectibleIndex(selectedCltList[0].ID) - 1;
                            UpdateObjectHierarchyList(selectedCltList[0].gameObject);
                            SelectItem();
                        }
                        //if the selected object is not in DB
                        else
                        {
                            selectID = -1;
                            UpdateObjectHierarchyList(selectedCltList[0].gameObject);
                        }
                    }
                }
                //selected multiple editable object
                else
                {
                    selectID = -1;
                    UpdateObjectHierarchyList(selectedCltList[0].gameObject);
                }

                srlObj = new SerializedObject(filtered);
            }

            Repaint();
        }
    }
}

