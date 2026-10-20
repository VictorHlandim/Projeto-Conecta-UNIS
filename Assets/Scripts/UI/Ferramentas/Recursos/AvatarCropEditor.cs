
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class AvatarCropEditor : MonoBehaviour,
    IBeginDragHandler, IDragHandler
{
    [Header("Interface")]
    [SerializeField] private RectTransform cropViewport;
    [SerializeField] private RawImage sourceImage;
    [SerializeField] private Slider zoomSlider;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Button confirmButton;

    [Header("Exportacao")]
    [SerializeField] private int outputSize = 512;
    [SerializeField] private int maxInputSize = 4096;

    public event Action<byte[]> OnAvatarReady;

    private Texture2D sourceTexture;
    private RectTransform imageRect;

    private float baseScale;
    private float zoom = 1f;
    private Vector2 dragStart;
    private Vector2 imageStartPosition;
    private bool hasImage;

    private void Awake()
    {
        if (cropViewport == null ||
            sourceImage == null ||
            zoomSlider == null ||
            statusText == null ||
            confirmButton == null)
        {
            Debug.LogError(
                "AvatarCropEditor: faltam referências no Inspector.",
                this
            );
            enabled = false;
            return;
        }

        imageRect = sourceImage.rectTransform;

        zoomSlider.minValue = 1f;
        zoomSlider.maxValue = 3f;
        zoomSlider.value = 1f;

        zoomSlider.onValueChanged.RemoveListener(SetZoom);
        zoomSlider.onValueChanged.AddListener(SetZoom);

        confirmButton.onClick.RemoveListener(ConfirmCrop);
        confirmButton.onClick.AddListener(ConfirmCrop);

        confirmButton.interactable = false;
    }
    // Chamado pelo botao "Escolher foto".
    public void SelectPhoto()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        NativeGallery.GetImageFromGallery(path =>
        {
            if (!string.IsNullOrEmpty(path))
                StartCoroutine(LoadImageFromPath(path));
        }, "Escolha sua foto de perfil", "image/*");
#else
        var paths = SFB.StandaloneFileBrowser.OpenFilePanel(
            "Escolha sua foto",
            "",
            new[]
            {
                new SFB.ExtensionFilter(
                    "Imagens", "png", "jpg", "jpeg"
                )
            },
            false
        );

        if (paths != null && paths.Length > 0 &&
            !string.IsNullOrEmpty(paths[0]))
        {
            StartCoroutine(LoadImageFromPath(paths[0]));
        }
#endif
    }

    private IEnumerator LoadImageFromPath(string path)
    {
        statusText.text = "Carregando imagem...";

        byte[] bytes;

        try
        {
            bytes = File.ReadAllBytes(path);
        }
        catch (Exception e)
        {
            statusText.text = "Nao foi possivel ler a imagem.";
            Debug.LogException(e);
            yield break;
        }

        Texture2D texture = new Texture2D(
            2, 2, TextureFormat.RGBA32, false
        );

        if (!texture.LoadImage(bytes))
        {
            Destroy(texture);
            statusText.text = "Arquivo de imagem invalido.";
            yield break;
        }

        if (texture.width > maxInputSize ||
            texture.height > maxInputSize)
        {
            Texture2D resized = ResizeTexture(
                texture, maxInputSize
            );

            Destroy(texture);
            texture = resized;
        }

        if (sourceTexture != null)
            Destroy(sourceTexture);

        sourceTexture = texture;
        sourceImage.texture = sourceTexture;

        // A imagem deve cobrir totalmente a area quadrada.
        float viewportWidth = cropViewport.rect.width;
        float viewportHeight = cropViewport.rect.height;

        imageRect.anchorMin = new Vector2(0.5f, 0.5f);
        imageRect.anchorMax = new Vector2(0.5f, 0.5f);
        imageRect.pivot = new Vector2(0.5f, 0.5f);
        imageRect.anchoredPosition = Vector2.zero;
        imageRect.sizeDelta = new Vector2(
            sourceTexture.width, sourceTexture.height
        );

        baseScale = Mathf.Max(
            viewportWidth / sourceTexture.width,
            viewportHeight / sourceTexture.height
        );

        zoom = 1f;
        zoomSlider.SetValueWithoutNotify(1f);
        imageRect.localScale = Vector3.one * baseScale;

        hasImage = true;
        confirmButton.interactable = true;
        statusText.text = "Arraste e ajuste o zoom.";
        ClampImagePosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!hasImage) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            cropViewport,
            eventData.position,
            eventData.pressEventCamera,
            out dragStart
        );

        imageStartPosition = imageRect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!hasImage) return;

        if (RectTransformUtility
            .ScreenPointToLocalPointInRectangle(
                cropViewport,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 currentPoint))
        {
            imageRect.anchoredPosition =
                imageStartPosition + currentPoint - dragStart;

            ClampImagePosition();
        }
    }

    public void SetZoom(float value)
    {
        if (!hasImage) return;

        zoom = value;
        imageRect.localScale = Vector3.one * baseScale * zoom;
        ClampImagePosition();
    }

    private void ClampImagePosition()
    {
        if (!hasImage) return;

        float displayedWidth =
            sourceTexture.width * imageRect.localScale.x;

        float displayedHeight =
            sourceTexture.height * imageRect.localScale.y;

        float halfViewportWidth = cropViewport.rect.width / 2f;
        float halfViewportHeight = cropViewport.rect.height / 2f;

        Vector2 p = imageRect.anchoredPosition;

        float maxX = Mathf.Max(
            0f, (displayedWidth - 2f * halfViewportWidth) / 2f
        );

        float maxY = Mathf.Max(
            0f, (displayedHeight - 2f * halfViewportHeight) / 2f
        );

        p.x = Mathf.Clamp(p.x, -maxX, maxX);
        p.y = Mathf.Clamp(p.y, -maxY, maxY);

        imageRect.anchoredPosition = p;
    }


    public void ConfirmCrop()
    {
        if (!hasImage) return;

        try
        {
            byte[] png = ExportSquareCrop();

            if (png == null || png.Length == 0)
            {
                statusText.text = "Falha ao gerar a imagem.";
                return;
            }

            // Atualize o status antes de disparar o upload.
            statusText.text = "Preparando envio...";

            OnAvatarReady?.Invoke(png);
        }
        catch (Exception e)
        {
            statusText.text = "Erro ao gerar o recorte.";
            Debug.LogException(e);
        }
    }


    private byte[] ExportSquareCrop()
    {
        int size = Mathf.Max(64, outputSize);

        Texture2D result = new Texture2D(
            size, size, TextureFormat.RGBA32, false
        );

        float displayedWidth =
            sourceTexture.width * imageRect.localScale.x;

        float displayedHeight =
            sourceTexture.height * imageRect.localScale.y;

        float left = imageRect.anchoredPosition.x
            - displayedWidth / 2f;

        float top = imageRect.anchoredPosition.y
            + displayedHeight / 2f;

        float viewLeft = -cropViewport.rect.width / 2f;
        float viewTop = cropViewport.rect.height / 2f;

        float sourceLeft =
            (viewLeft - left) / displayedWidth * sourceTexture.width;

        float sourceTop =
            (top - viewTop) / displayedHeight * sourceTexture.height;

        float sourceWidth =
            cropViewport.rect.width / displayedWidth
            * sourceTexture.width;

        float sourceHeight =
            cropViewport.rect.height / displayedHeight
            * sourceTexture.height;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float sx = sourceLeft
                    + ((x + 0.5f) / size) * sourceWidth;

                float sy = sourceTop
                    + ((y + 0.5f) / size) * sourceHeight;

                float u = Mathf.Clamp01(sx / sourceTexture.width);
                float v = Mathf.Clamp01(
                    1f - sy / sourceTexture.height
                );

                result.SetPixel(
                    x, y, sourceTexture.GetPixelBilinear(u, v)
                );
            }
        }

        result.Apply();
        byte[] png = result.EncodeToPNG();

        Destroy(result);

        return png;
    }

    private Texture2D ResizeTexture(Texture2D original, int maxSize)
    {
        float factor = Mathf.Min(
            1f,
            maxSize / (float)original.width,
            maxSize / (float)original.height
        );

        int width = Mathf.Max(
            1, Mathf.RoundToInt(original.width * factor)
        );

        int height = Mathf.Max(
            1, Mathf.RoundToInt(original.height * factor)
        );

        RenderTexture rt = RenderTexture.GetTemporary(
            width, height, 0, RenderTextureFormat.ARGB32
        );

        RenderTexture previous = RenderTexture.active;

        Graphics.Blit(original, rt);
        RenderTexture.active = rt;

        Texture2D resized = new Texture2D(
            width, height, TextureFormat.RGBA32, false
        );

        resized.ReadPixels(
            new Rect(0, 0, width, height), 0, 0, false
        );
        resized.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return resized;
    }

    private void OnDestroy()
    {
        if (sourceTexture != null)
            Destroy(sourceTexture);

        if (zoomSlider != null)
            zoomSlider.onValueChanged.RemoveListener(SetZoom);

        if (confirmButton != null)
            confirmButton.onClick.RemoveListener(ConfirmCrop);
    }
}
