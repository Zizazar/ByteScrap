using UnityEngine;
using UnityEditor;

/// <summary>
/// Сгенерированно с помощью Gemini
/// Этот скрипт добавляет иконку C# рядом с
/// любым GameObject в иерархии, у которого есть скрипты.
/// Нажатие на иконку открывает скрипт в редакторе кода.
/// </summary>
[InitializeOnLoad] // Этот атрибут заставляет класс запускаться при загрузке редактора
public class HierarchyScriptOpener
{
    private static readonly GUIContent iconContent;

    // Статический конструктор вызывается один раз при загрузке Unity
    static HierarchyScriptOpener()
    {
        // Загружаем стандартную иконку C# скрипта
        iconContent = EditorGUIUtility.IconContent("cs Script Icon");
        iconContent.tooltip = "Открыть скрипт(ы)";

        // Подписываемся на событие отрисовки каждого элемента в окне иерархии
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
    }

    private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
    {
        // Получаем сам GameObject по его ID
        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (go == null)
        {
            return;
        }

        // Ищем *все* компоненты MonoBehaviour (скрипты) на этом объекте
        MonoBehaviour[] scripts = go.GetComponents<MonoBehaviour>();

        // Если скриптов нет, ничего не рисуем
        if (scripts.Length == 0)
        {
            return;
        }

        // --- Отрисовка кнопки ---

        // Определяем позицию и размер для нашей кнопки.
        // Мы разместим ее справа, с небольшим отступом.
        float buttonWidth = 18f;
        float buttonHeight = 16f; // Стандартная высота строки в иерархии
        Rect buttonRect = new Rect(
            selectionRect.xMax - buttonWidth - 2f, // xMax - это правый край
            selectionRect.y,
            buttonWidth,
            buttonHeight
        );

        // Рисуем кнопку. Используем стиль GUI.skin.label,
        // чтобы она была плоской и похожей на иконку.
        if (GUI.Button(buttonRect, iconContent, GUI.skin.label))
        {
            // --- Логика при нажатии ---

            if (scripts.Length == 1)
            {
                // Если скрипт всего один, сразу его открываем
                OpenScript(scripts[0]);
            }
            else
            {
                // Если скриптов несколько, показываем контекстное меню для выбора
                ShowScriptSelectionMenu(scripts);
            }

            // "Съедаем" событие клика, чтобы
            // сам GameObject не выделился (или не развыделился)
            Event.current.Use();
        }
    }

    /// <summary>
    /// Открывает файл скрипта в ассоциированном редакторе кода (VS, Rider и т.д.)
    /// </summary>
    private static void OpenScript(MonoBehaviour script)
    {
        if (script == null) return;

        // Получаем ассет MonoScript из компонента MonoBehaviour
        MonoScript monoScript = MonoScript.FromMonoBehaviour(script);
        if (monoScript != null)
        {
            // Открываем ассет
            AssetDatabase.OpenAsset(monoScript);
        }
    }

    /// <summary>
    /// Показывает выпадающее меню со списком всех скриптов на объекте
    /// </summary>
    private static void ShowScriptSelectionMenu(MonoBehaviour[] scripts)
    {
        GenericMenu menu = new GenericMenu();

        foreach (var script in scripts)
        {
            MonoScript monoScript = MonoScript.FromMonoBehaviour(script);
            if (monoScript != null)
            {
                // Важно: создаем локальную копию для лямбда-выражения,
                // иначе все пункты меню будут ссылаться на последний скрипт в цикле.
                MonoScript scriptToOpen = monoScript;
                
                menu.AddItem(new GUIContent(monoScript.name), false, () =>
                {
                    AssetDatabase.OpenAsset(scriptToOpen);
                });
            }
        }

        // Показываем меню под курсором
        menu.ShowAsContext();
    }
}