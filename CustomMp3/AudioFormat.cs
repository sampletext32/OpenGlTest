public class AudioFormat
{
    private int sampleRate;
    private int sampleSizeInBits;
    private int channels;
    private bool signed;
    private bool bigEndian;

    public AudioFormat(int sampleRate, int sampleSizeInBits, int channels, bool signed, bool bigEndian)
    {
        this.sampleRate = sampleRate;
        this.sampleSizeInBits = sampleSizeInBits;
        this.channels = channels;
        this.signed = signed;
        this.bigEndian = bigEndian;
    }
}