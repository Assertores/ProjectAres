using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PPBC {
    [Serializable]
    [PostProcess(typeof(OutlinePPSRenderer), PostProcessEvent.AfterStack, "Custom/OutlinePPS")]
    public sealed class OutlinePPS : PostProcessEffectSettings {
        [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
        public FloatParameter blend = new FloatParameter { value = 0.5f };

        public Material mask; // global instac


    }

    public sealed class OutlinePPSRenderer : PostProcessEffectRenderer<OutlinePPS> {
        public override void Render(PostProcessRenderContext context) {
            //var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Grayscale"));
            //sheet.properties.SetFloat("_Blend", settings.blend);
            //context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

            var rt = RenderTexture.GetTemporary(0, 0, 24);
            var active = RenderTexture.active;
            RenderTexture.active = rt;

            context.command.Clear();

            foreach (var player in Player.s_references) {
                foreach (var renderer in player.GetComponentsInChildren<Renderer>()) {
                    //renderer.SetPropertyBlock()//farbe aus dem spieler auslesen
                    context.command.DrawRenderer(renderer, this.settings.mask);
                }
            }
            //shader set global textur in shader übergeben
            //context blid auf bildschirm
            //foreach(outline berechenen)
            RenderTexture.ReleaseTemporary(rt);
            RenderTexture.active = active;
        }
    }
}