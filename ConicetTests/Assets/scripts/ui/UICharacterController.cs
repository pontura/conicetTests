using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Conicet.UI
{
    public class UICharacterController : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Sprite[] characters;

        private void Start()
        {
            Events.ChangeCharacter += ChangeCharacter;
        }
        private void OnDestroy()
        {
            Events.ChangeCharacter -= ChangeCharacter;
        }
        int lastID = -1;
        private void ChangeCharacter(int id)
        {
            if (lastID == id) return;
            lastID = id;
            if (id < 1)  id = 1;
            else if (id > 2)  id = 2;
            print("ChangeCharacter " + id);
            image.sprite = characters[id-1];
            image.SetNativeSize();
        }
    }
}
