using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;

namespace MidiEventReader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Reading Devices and subscribing to events...");
            string deviceSelector = MidiInPort.GetDeviceSelector();
            var midiInputDevices = await DeviceInformation.FindAllAsync(deviceSelector);
            var device = midiInputDevices.FirstOrDefault();
            Console.WriteLine($"Reading from device {device.Name}");

            var port = await MidiInPort.FromIdAsync(device.Id);
            port.MessageReceived += MidiDeviceService_MessageReceived;

            Console.ReadKey();
        }

        public static void MidiDeviceService_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            var message = args.Message;
            if (message.Type == MidiMessageType.NoteOn)
            {
                var noteOn = (MidiNoteOnMessage)message;
                Console.WriteLine($"Key {noteOn.Note} pressed!");
            }
            else if (message.Type == MidiMessageType.NoteOff)
            {
                var noteOff = (MidiNoteOffMessage)message;
                Console.WriteLine($"Key {noteOff.Note} depressed!");
            }
        }
    }
}
