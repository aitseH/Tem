using UnityEditor;
using UnityEngine;

using System;

using System.Collections;
using System.Collections.Generic;

using Tem;

namespace Tem
{

    public class EffectEditorWindow : TemEditorWindow
    {

        private static EffectEditorWindow window;

        public static void Init()
        {

            window = (EffectEditorWindow)EditorWindow.GetWindow(typeof(EffectEditorWindow), false, "Effect Editor");
            window.minSize = new Vector2(400, 300);

            LoadDB();

            window.SetupCallback();
        }

        public void SetupCallback()
        {
            shiftItemUpCallback = this.ShiftItemUp;
            shiftItemDownCallback = this.ShiftItemDown;
            deleteItemCallback = this.DeleteItem;
        }

        public override bool OnGUI()
        {
            if (!base.OnGUI()) return true;

            if (window == null) Init();

            List<Effect> effectList = effectDB.effectList;

            Undo.RecordObject(this, "window");
            Undo.RecordObject(effectDB, "EffectDB");

            if (GUI.Button(new Rect(Math.Max(260, window.position.width - 120), 5, 100, 25), "Save")) SetDirtyTem();

            if (GUI.Button(new Rect(5, 5, 120, 25), "Create New")) Select(NewItem());
            if (effectList.Count > 0 && GUI.Button(new Rect(130, 5, 100, 25), "Clone Selected")) Select(NewItem(selectID));

            float startX = 5; float startY = 55;

            if (minimiseList)
            {
                if (GUI.Button(new Rect(startX, startY - 20, 30, 18), ">>")) minimiseList = false;
            }
            else
            {
                if (GUI.Button(new Rect(startX, startY - 20, 30, 18), "<<")) minimiseList = true;
            }

            Vector2 v2 = DrawEffectList(startX, startY, effectList);

            startX = v2.x + 25;

            if (effectList.Count == 0) return true;

            Rect visibleRect = new Rect(startX, startY, window.position.width - startX - 10, window.position.height - startY - 5);
            Rect contentRect = new Rect(startX, startY, contentWidth - startY, contentHeight);

            scrollPos = GUI.BeginScrollView(visibleRect, scrollPos, contentRect);

            //float cachedX=startX;
            v2 = DrawEffectConfigurator(startX, startY, effectList[selectID]);
            contentWidth = v2.x + 35;
            contentHeight = v2.y - 55;

            GUI.EndScrollView();

            if (GUI.changed) SetDirtyTem();

            return true;
        }


        Vector2 DrawEffectConfigurator(float startX, float startY, Effect effect)
        {
            TemEditorUtility.DrawSprite(new Rect(startX, startY, 60, 60), effect.icon);
            startX += 65;

            cont = new GUIContent("Name:", "The effect name to be displayed in game");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY / 2, width, height), cont);
            effect.name = EditorGUI.TextField(new Rect(startX + spaceX - 65, startY, width - 5, height), effect.name);

            cont = new GUIContent("Icon:", "The effect icon to be displayed in game, must be a sprite");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            effect.icon = (Sprite)EditorGUI.ObjectField(new Rect(startX + spaceX - 65, startY, width - 5, height), effect.icon, typeof(Sprite), false);

            startX -= 65;
            startY += 10 + spaceY;  //cachedY=startY;

            startY += 5;

            cont = new GUIContent("invincible:", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, width, height), cont);
            effect.invincible = EditorGUI.Toggle(new Rect(startX + spaceX, startY, 40, height), effect.invincible);


            GUIStyle style = new GUIStyle("TextArea");
            style.wordWrap = true;
            cont = new GUIContent("Effect description (to be used in runtime): ", "");
            EditorGUI.LabelField(new Rect(startX, startY += spaceY, 400, 20), cont);
            effect.desp = EditorGUI.TextArea(new Rect(startX, startY + spaceY - 3, 270, 150), effect.desp, style);
            return new Vector2(startX, startY + 200);
        }


        protected Vector2 DrawEffectList(float startX, float startY, List<Effect> effectList)
        {
            List<Item> list = new List<Item>();
            for (int i = 0; i < effectList.Count; i++)
            {
                Item item = new Item(effectList[i].ID, effectList[i].name, effectList[i].icon);
                list.Add(item);
            }
            return DrawList(startX, startY, window.position.width, window.position.height, list);
        }

        int NewItem(int cloneID = -1)
        {
            Effect effect = null;
            if (cloneID == -1)
            {
                effect = new Effect();
                effect.name = "New Effect";
            }
            else
            {
                effect = effectDB.effectList[selectID].Clone();
            }
            effect.ID = GenerateNewID(effectIDList);
            effectIDList.Add(effect.ID);

            effectDB.effectList.Add(effect);

            UpdateLabel_Effect();

            return effectDB.effectList.Count - 1;
        }


        void DeleteItem()
        {
            effectIDList.Remove(effectDB.effectList[deleteID].ID);
            effectDB.effectList.RemoveAt(deleteID);

            UpdateLabel_Effect();
        }
        void ShiftItemUp() { if (selectID > 0) ShiftItem(-1); }

        void ShiftItemDown() { if (selectID < effectDB.effectList.Count - 1) ShiftItem(1); }

        void ShiftItem(int dir)
        {
            Effect effect = effectDB.effectList[selectID];
            effectDB.effectList[selectID] = effectDB.effectList[selectID + dir];
            effectDB.effectList[selectID + dir] = effect;
            selectID += dir;
        }

    }
}