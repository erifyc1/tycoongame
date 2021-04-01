public enum BlendMode
{
    Opaque,
    Cutout,
    Fade,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
    Transparent // Physically plausible transparency mode, implemented as alpha pre-multiply
}
