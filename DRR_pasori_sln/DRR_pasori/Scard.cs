/*
The MIT License (MIT)

Copyright (c) <2015> <Soft-Studio K.K. info@soft-studio.jp>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace DRR_pasori
{
    class Scard
    {
         [DllImport("winscard.dll")]
         static extern uint SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, out IntPtr phContext);

        [DllImport("winscard.dll", EntryPoint = "SCardListReadersW", CharSet = CharSet.Unicode)]
        static extern uint SCardListReaders(
          IntPtr hContext,
          byte[] mszGroups,
          byte[] mszReaders,
          ref UInt32 pcchReaders
          );
 
        [DllImport("WinScard.dll")]
        static extern uint SCardReleaseContext(IntPtr phContext);
 
        [DllImport("winscard.dll", EntryPoint = "SCardConnectW", CharSet = CharSet.Unicode)]
        static extern uint SCardConnect(
             IntPtr hContext,
             string szReader, 
             uint dwShareMode,
             uint dwPreferredProtocols,
             ref IntPtr phCard,
             ref IntPtr pdwActiveProtocol);
 
        [DllImport("WinScard.dll")]
        static extern uint SCardDisconnect(IntPtr hCard, int Disposition);
 
        [StructLayout(LayoutKind.Sequential)]
        internal class SCARD_IO_REQUEST
        {
            internal uint dwProtocol;
            internal int cbPciLength;
            public SCARD_IO_REQUEST()
            {
                dwProtocol = 0;
            }
        }
 
        [DllImport("winscard.dll")]
        static extern uint SCardTransmit(IntPtr hCard, IntPtr pioSendRequest, byte[] SendBuff, int SendBuffLen, SCARD_IO_REQUEST pioRecvRequest,
                byte[] RecvBuff, ref int RecvBuffLen);
 
        [DllImport("winscard.dll")]
        static extern uint SCardControl(IntPtr hCard, int controlCode, byte[] inBuffer, int inBufferLen, byte[] outBuffer, int outBufferLen, ref int bytesReturned);
 
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SCARD_READERSTATE
        {
            /// <summary>
            /// Reader
            /// </summary>
            internal string szReader;
            /// <summary>
            /// User Data
            /// </summary>
            internal IntPtr pvUserData;
            /// <summary>
            /// Current State
            /// </summary>
            internal UInt32 dwCurrentState;
            /// <summary>
            /// Event State/ New State
            /// </summary>
            internal UInt32 dwEventState;
            /// <summary>
            /// ATR Length
            /// </summary>
            internal UInt32 cbAtr;
            /// <summary>
            /// Card ATR
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            internal byte[] rgbAtr;
        }
 
        [DllImport("winscard.dll", EntryPoint = "SCardGetStatusChangeW", CharSet = CharSet.Unicode)]
        static extern uint SCardGetStatusChange(IntPtr hContext, int dwTimeout, [In, Out] SCARD_READERSTATE[] rgReaderStates, int cReaders);
 
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern  IntPtr LoadLibrary(string lpFileName);
 
        [DllImport("kernel32.dll")]
        static extern void FreeLibrary(IntPtr handle);
 
        [DllImport("kernel32.dll")]
        static private extern  IntPtr GetProcAddress(IntPtr handle, string procName);

        public String name { get; private set; }
        public String kana  { get; private set; }
        public String tusyo { get; private set; }
        public String toitsu { get; private set; }
        public String birth { get; private set; }
        public String address { get; private set; }
        public String kofuday { get; private set; }
        public String syoukai { get; private set; }
        public String kubun { get; private set; }
        public String yukoday { get; private set; }
        public String joken1 { get; private set; }
        public String joken2 { get; private set; }
        public String joken3 { get; private set; }
        public String joken4 { get; private set; }
        public String koanname { get; private set; }
        public String menkyonumber { get; private set; }

        public String nisyogenday { get; private set; }	//	免許の年月日(二・小・原)(元号(注6)YYMMDD)(注9)
        public String hokaday { get; private set; } //免許の年月日(他)(元号(注6)YYMMDD)(注9)
        public String nisyuday { get; private set; }//	免許の年月日(二種)(元号(注6)YYMMDD)(注9)
        public String ogataday { get; private set; } //免許の年月日(大型)(元号(注6)YYMMDD)(注9)
        public String futuday { get; private set; }// 免許の年月日(普通)(元号(注6)YYMMDD)(注9)
        public String daitokuday { get; private set; } //免許の年月日(大特)(元号(注6)YYMMDD)(注9)
        public String daijiniday { get; private set; }// 免許の年月日(大自二)(元号(注6)YYMMDD)(注9)
        public String futujiniday { get; private set; }//免許の年月日(普自二)(元号(注6)YYMMDD)(注9)
        public String kotokuday { get; private set; }// 免許の年月日(小特)(元号(注6)YYMMDD)(注9)
        public String gentukiday { get; private set; }// 免許の年月日(原付)(元号(注6)YYMMDD)(注9)
        public String keninday { get; private set; }//	免許の年月日(け引)(元号(注6)YYMMDD)(注9)
        public String daijiday { get; private set; }// 免許の年月日(大二)(元号(注6)YYMMDD)(注9)
        public String fujiday { get; private set; }//免許の年月日(普二)(元号(注6)YYMMDD)(注9)
        public String daitokuji { get; private set; }//免許の年月日(大特二)(元号(注6)YYMMDD)(注9)
        public String keninniday { get; private set; }//免許の年月日(け引二)(元号(注6)YYMMDD)(注9)
        public String chuday { get; private set; }// 免許の年月日(中型)(元号(注6)YYMMDD)(注9,注12)
        public String chuniday { get; private set; }//免許の年月日(中二)(元号(注6)YYMMDD)(注9,注12)

        public String honseki { get; private set; }

        public byte[] Gaiji1 { get; private set; }
        public byte[] Gaiji2 { get; private set; }
        public byte[] picture { get; private set; }

        public String errormsg { get; private set; }

        public static byte kisaijikoutuiki;//記載事項変更等 追記

        volatile static bool quit = false;

        private byte[] pass1byte;
        private byte[] pass2byte;


        static void KeyCheck(object userState)
        {
//            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            quit = false;
        }


        public bool start(String pass1,String pass2)
        {
            IntPtr hContext = establishContext();
            bool result =false;
            pass1byte = new byte[4];
            pass2byte = new byte[4];

            pass1byte = System.Text.Encoding.ASCII.GetBytes(pass1);
            pass2byte = System.Text.Encoding.ASCII.GetBytes(pass2);
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(KeyCheck), null);
 
                List<string> readersList = getReaders(hContext);
 
                Debug.WriteLine(readersList.Count + "台のリーダーがあります。");
                foreach (string readerName in readersList)
                {
                    Debug.WriteLine("リーダー:" + readerName);
                }
 
 
                SCARD_READERSTATE[] readerStateArray = initializeReaderState(hContext, readersList);
                updateCurrentState(readerStateArray);
                while(!quit)
                {
                    const int SCARD_STATE_UNAWARE = 0x0000;
                    const int SCARD_STATE_CHANGED  =   0x00000002;// This implies that there is a
                                            // difference between the state
                                            // believed by the application, and
                                            // the state known by the Service
                                            // Manager.  When this bit is set,
                                            // the application may assume a
                                            // significant state change has
                                            // occurred on this reader.
                    const int SCARD_STATE_PRESENT = 0x00000020;// This implies that there is a card
                                            // in the reader.
                    const UInt32 SCARD_STATE_EMPTY = 0x00000010;  // This implies that there is not
                                            // card in the reader.  If this bit
                                            // is set, all the following bits
                                            // will be clear.
                    bool cardEx = false;

                    try
                    {
                        for (int idx = 0; idx < readerStateArray.Length; idx++)
                        {
                            readerStateArray[idx].dwCurrentState = SCARD_STATE_UNAWARE;
                        }
                        waitReaderStatusChange(hContext, readerStateArray, 1000);
                        for (int idx = 0; idx < readerStateArray.Length; idx++)
                        {
                            if ((readerStateArray[idx].dwEventState & SCARD_STATE_PRESENT) == SCARD_STATE_PRESENT)
                            {
                                cardEx = true;
                                break;
                            }
                        }
                        updateCurrentState(readerStateArray);
                        if (cardEx == false)
                        {
                            waitReaderStatusChange(hContext, readerStateArray, 1000);
                        }
                        for (int idx = 0; idx < readerStateArray.Length; idx++)
                        {
                            uint eventState = readerStateArray[idx].dwEventState;
                            uint changedStateMask = eventState ^ readerStateArray[idx].dwCurrentState;
                            if ((readerStateArray[idx].dwEventState & SCARD_STATE_PRESENT) == SCARD_STATE_PRESENT)
                            {
                                ReadResult result2 = readCard(hContext, readerStateArray[idx].szReader);
                                result = SendCommand(hContext, readerStateArray[idx].szReader);
                                quit = true;
                            }
                            if ((eventState & SCARD_STATE_EMPTY) != 0)
                            {
                                Debug.WriteLine("リーダー " + readerStateArray[idx].szReader + " カードが外されました。");
                            }
                            updateCurrentState(readerStateArray);
                        }
                    }
                    catch (TimeoutException e)
                    {
                        // 無視
                    }
 
                }
 
            }
            finally
            {
                uint ret = SCardReleaseContext(hContext);
            }
            return result;

        }

 
        private bool SendCommand(IntPtr hContext, string readerName)
        {
        	int dwBufferSize;
    	    int dwResponseSize;
	        long  lResult;
	        byte[] response =  new byte[2048];
            byte[] commnadMF01 = { 0x00, 0xa4, 0x00, 0x00 };
            byte[] commnadVREF1 = {0x00,0x20,0x00,0x81};
            byte[] commnadVREF2 = {0x00,0x20,0x00,0x82};
            byte[] commnadVRPIN1 = {0x00,0x20,0x00,0x81,0x00,0x00,0x00,0x00,0x00};
            byte[] commnadVRPIN2 = {0x00,0x20,0x00,0x82,0x00,0x00,0x00,0x00,0x00};
            byte[] commnadDF01 = {0x00,0xA4,0x04,0x0C,0x10,0xA0,0x00,0x00,0x02,0x31,0x01,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
            byte[] commnadDF02 = {0x00,0xA4,0x04,0x0C,0x10,0xA0,0x00,0x00,0x02,0x31,0x02,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
            byte[] commnadEF01 = {0x00,0xA4,0x02,0x0C,0x02,0x00,0x01};
            byte[] commnadEF02 = {0x00,0xA4,0x02,0x0C,0x02,0x00,0x02};
            byte[] commnadEF03 = {0x00,0xA4,0x02,0x0C,0x02,0x00,0x03};
            byte[] commnadEF04 = {0x00,0xA4,0x02,0x0C,0x02,0x00,0x04};
            byte[] commnadEF05 = {0x00,0xA4,0x02,0x0C,0x02,0x00,0x05};
            byte[] commnadEF06 = {0x00,0xA4,0x02,0x0C,0x02,0x00,0x06};
            byte[] commnadREAD01 = {0x00,0xB0,0x00,0x00,0x00,0x03,0x70};
            byte[] commnadREAD02 = { 0x00, 0xB0, 0x00, 0x00, 0x52 };
            byte[] commnadREADPICT = { 0x00, 0xB0, 0x00, 0x00, 0x00, 0x07, 0xD0 };


            IntPtr SCARD_PCI_T1 = getPciT1();
            SCARD_IO_REQUEST ioRecv = new SCARD_IO_REQUEST();
            ioRecv.cbPciLength = 2048;
            IntPtr hCard = connect(hContext, readerName);

        	dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadMF01, commnadMF01.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }

            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadVREF1, commnadVREF1.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }

            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadVREF2, commnadVREF2.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }

            commnadVRPIN1[4]=4;
            commnadVRPIN1[5] = pass1byte[0];
            commnadVRPIN1[6] = pass1byte[1];
            commnadVRPIN1[7] = pass1byte[2];
            commnadVRPIN1[8] = pass1byte[3];
            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadVRPIN1, commnadVRPIN1.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            if (response[0] == (byte)0x63)
            {
                errormsg = "暗所番号１が違います";
                return false;
            }
            commnadVRPIN2[4] = 4;
            commnadVRPIN2[5] = pass2byte[0];
            commnadVRPIN2[6] = pass2byte[1];
            commnadVRPIN2[7] = pass2byte[2];
            commnadVRPIN2[8] = pass2byte[3];
            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadVRPIN2, commnadVRPIN2.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            if (response[0] == (byte)0x63)
            {
                errormsg = "暗所番号２が違います";
                return false;
            }

            
            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadDF01, commnadDF01.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadEF01, commnadEF01.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadREAD01, commnadREAD01.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            parse_tag(response);



            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadEF02, commnadEF02.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadREAD02, commnadREAD02.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            parse_tag(response);



            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadDF02, commnadDF02.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadEF01, commnadEF01.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            dwResponseSize = response.Length;
            lResult = SCardTransmit(hCard, SCARD_PCI_T1, commnadREADPICT, commnadREADPICT.Length, ioRecv, response, ref dwResponseSize);
            if (lResult != SCARD_S_SUCCESS)
            {
                return false;
            }
            parse_tag_picture(response);

            quit = true;
            return response[dwResponseSize - 2] == 0x90 && response[dwResponseSize - 1] == 0x00;
        }



        private void parse_tag(byte[] data)
        {
            bool bLoop = true;
            int index = 0;
            int length = 0;
            do
            {
                switch (data[index])
                {
                    case 0x11:
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        break;

                    case 0x12:	//名前
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        name = convertByteToJisString(data, index, length);
                        break;
                    case 0x13:	//カナ
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        kana = convertByteToJisString(data, index, length);
                        break;
                    case 0x14:	//通称名
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        tusyo = convertByteToJisString(data, index, length);
                        break;
                    case 0x15:	//統一氏名
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        toitsu = convertByteToJisString(data, index, length);
                        break;
                    case 0x16:	//生年月日
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        birth = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x17:	//住所
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        address = convertByteToJisString(data, index, length);
                        break;
                    case 0x18:	//交付年月日
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        kofuday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x19:	//照会番号
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        syoukai = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x1A:	//免許の色区分
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        kubun = convertByteToJisString(data, index, length);
                        break;
                    case 0x1B:	//有効期間の末日
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        yukoday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x1C:	//免許の条件1
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        joken1 = convertByteToJisString(data, index, length);
                        break;
                    case 0x1D:	//免許の条件2
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        joken2 = convertByteToJisString(data, index, length);
                        break;
                    case 0x1E:	//免許の条件3
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        joken3 = convertByteToJisString(data, index, length);
                        break;
                    case 0x1F:	//免許の条件4
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        joken4 = convertByteToJisString(data, index, length);
                        break;
                    case 0x20:	//公安委員会名
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        koanname = convertByteToJisString(data, index, length);
                        break;
                    case 0x21:	//免許証の番号
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        menkyonumber = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x22:	//免許の年月日(二・小・原)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        nisyogenday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x23:	//免許の年月日(他)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        hokaday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x24:	//免許の年月日(二種)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        nisyuday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x25:	//免許の年月日(大型)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        ogataday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x26:	//免許の年月日(普通)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        futuday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x27:	//免許の年月日(大特)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        daitokuday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x28:	//免許の年月日(大自二)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        daijiniday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x29:	//免許の年月日(普自二)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        futujiniday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x2A:	//免許の年月日(小特)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        kotokuday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x2B:	//免許の年月日(原付)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        gentukiday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x2C:	//免許の年月日(け引)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        keninday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x2D:	//免許の年月日(大二)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        daijiday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x2E:	//免許の年月日(普二)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        fujiday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x2F:	//免許の年月日(大特二)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        daitokuji = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x30:	//免許の年月日(け引二)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        keninniday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x31:	// 免許の年月日(中型)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        chuday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x32:	// 免許の年月日(中二)
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        chuniday = convertByteToAsciiString(data, index, length);
                        break;
                    case 0x41:	//本籍
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        honseki = convertByteToJisString(data, index, length);
                        break;
                    case 0x48:	//外字１
                        index++;
                        length = data[index++];
                        if (length == 0)
                        {
                            Gaiji1 = new byte[1];
                            break;
                        }
                        Gaiji1 = new byte[length];
                        break;
                    case 0x49:	//外字１
                        index++;
                        length = data[index++];
                        if (length == 0)
                        {
                            Gaiji2 = new byte[1];
                            break;
                        }
                        Gaiji2 = new byte[length];
                        break;
                    case 0x50:	//追記の有無
                        index++;
                        length = data[index++];
                        if (length == 0) break;
                        kisaijikoutuiki = data[index];
                        break;
                    default:
                        bLoop = false;
                        break;
                }


                index = index + length;


            } while (bLoop);

        }
         String convertByteToJisString(byte[] data, int index, int length)
        {
            byte[] temp = new byte[length+3];
            byte[] kanjiin = { 0x1B, 0x24, 0x42 };
            Array.Copy(kanjiin, 0, temp, 0,3);
            Array.Copy(data, index, temp, 3, length);
            String str = System.Text.Encoding.GetEncoding(50220).GetString(temp);
            Debug.WriteLine(str);
            return str;
        }
         String convertByteToAsciiString(byte[] data, int index, int length)
        {
            byte[] temp = new byte[length];
            Array.Copy(data, index, temp, 0, length);
            String str = System.Text.Encoding.GetEncoding(50220).GetString(temp);
            Debug.WriteLine(str);
            return str;
        }



         ReadResult readCard(IntPtr hContext, string readerName)
        {
            IntPtr hCard = connect(hContext, readerName);
            string readerSerialNumber = readReaderSerialNumber(hCard);
            string cardId = readCardId(hCard);
            Debug.WriteLine(readerName + " (S/N " + readerSerialNumber + ") から、カードを読み取りました。" + cardId);
            disconnect(hCard);
 
            ReadResult result = new ReadResult();
            result.readerSerialNumber = readerSerialNumber;
            result.cardId = cardId;
            return result;
 
        }


         private void parse_tag_picture(byte[] data)
         {
             int index = 0;
             int length = 0;
             if ((data[index] == 0x5F) && (data[index + 1] == 0x40))
             {
                 index = index + 2;
                 length = (data[index + 1] << 8 & 0xFF00) + (data[index] & 0xFF);
                 index = index + 2;
                 picture = new byte[length];
                 int count = 0;
                 do
                 {
                     if ((data[index] == (byte)0xFF) && (data[index + 1] == (byte)0x4F))
                     {
                         break;
                     }
                     index++;
                     count++;
                 } while (count < length);
                 Array.Copy(data, index, picture,0, length);
             }
         }

 
 
         string readCardId(IntPtr hCard)
        {
            byte maxRecvDataLen = 64;
            byte[] recvBuffer = new byte[maxRecvDataLen + 2];
            byte[] sendBuffer = new byte[] { 0xff, 0xca, 0x00, 0x00, maxRecvDataLen };
            int recvLength = transmit(hCard, sendBuffer, recvBuffer);
 
            // recvBuffer の最後の2バイトはステータスコードなので、応じた処理が必要。
            // http://eternalwindows.jp/security/scard/scard07.html
 
            string cardId = BitConverter.ToString(recvBuffer, 0, recvLength - 2).Replace("-", "");
            return cardId;
        }
 
         string readReaderSerialNumber(IntPtr hCard)
        {
            int controlCode = 0x003136b0; // SCARD_CTL_CODE(3500) の値 
                                    // IOCTL_PCSC_CCID_ESCAPE
                                    // SONY SDK for NFC M579_PC_SC_2.1j.pdf 3.1.1 IOCTRL_PCSC_CCID_ESCAPE
            byte[] sendBuffer = new byte[] {0xc0, 0x08 }; // ESC_CMD_GET_INFO / Product Serial Number 
            byte[] recvBuffer = new byte[64];
            int recvLength = control(hCard, controlCode, sendBuffer, recvBuffer);
 
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            string serialNumber = asciiEncoding.GetString(recvBuffer, 0, recvLength - 1); // recvBufferには\0で終わる文字列が取得されるので、長さを-1する。
            return serialNumber;
        }
 
 
 
        const uint SCARD_S_SUCCESS = 0;
        const uint SCARD_E_NO_SERVICE = 0x8010001D;
        const uint SCARD_E_TIMEOUT = 0x8010000A;
 
        private IntPtr establishContext()
        {
            IntPtr hContext = IntPtr.Zero;
            const uint SCARD_SCOPE_USER = 0;
            const uint SCARD_SCOPE_TERMINAL = 1;
            const uint SCARD_SCOPE_SYSTEM = 2;
 
            uint ret = SCardEstablishContext(SCARD_SCOPE_USER, IntPtr.Zero, IntPtr.Zero, out hContext);
            if (ret != SCARD_S_SUCCESS)
            {
                string message;
                switch (ret)
                {
                    case SCARD_E_NO_SERVICE:
                        message = "Smart Cardサービスが起動されていません。";
                        break;
                    default:
                        message= "Smart Cardサービスに接続できません。code = " + ret;
                        break;
                }
                Debug.WriteLine(message);
                throw new NotSupportedException(message);
            }
            Debug.WriteLine("Smart Cardサービスに接続しました。");
            return hContext;
        }
 
         List<string> getReaders(IntPtr hContext)
        {
            uint pcchReaders = 0;
 
            // First call with 3rd parameter set to null gets readers buffer length.
            uint ret = SCardListReaders(hContext, null, null, ref pcchReaders);
            if (ret != SCARD_S_SUCCESS)
            {
                throw new ApplicationException("リーダーの情報が取得できません。code = " + ret);
            }
 
            byte[] mszReaders = new byte[pcchReaders * 2]; // 1文字2byte
 
            // Fill readers buffer with second call.
            ret = SCardListReaders(hContext, null, mszReaders, ref pcchReaders);
            if (ret != SCARD_S_SUCCESS)
            {
                throw new ApplicationException("リーダーの情報が取得できません。code = " + ret);
            }
 
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
            string readerNameMultiString = unicodeEncoding.GetString(mszReaders);
 
            Debug.WriteLine("リーダー名を\\0で接続した文字列: " + readerNameMultiString);
            Debug.WriteLine(" ");
 
            int len = (int)pcchReaders;
            char nullchar = (char)0;
            List<string> readersList = new List<string>();
 
            if (len > 0)
            {
                while (readerNameMultiString[0] != nullchar)
                {
                    int nullindex = readerNameMultiString.IndexOf(nullchar);   // Get null end character.
                    string readerName = readerNameMultiString.Substring(0, nullindex);
                    readersList.Add(readerName);
                    len = len - (readerName.Length + 1);
                    readerNameMultiString = readerNameMultiString.Substring(nullindex + 1, len);
                }
            }
            return readersList;
        }
 
 
         IntPtr connect(IntPtr hContext, string readerName)
        {
            const int SCARD_SHARE_SHARED = 0x00000002; // - This application will allow others to share the reader
            const int SCARD_SHARE_EXCLUSIVE = 0x00000001; // - This application will NOT allow others to share the reader
            const int SCARD_SHARE_DIRECT = 0x00000003; // - Direct control of the reader, even without a card
 
 
            const int SCARD_PROTOCOL_T0= 1; // - Use the T=0 protocol (value = 0x00000001)
            const int SCARD_PROTOCOL_T1= 2;// - Use the T=1 protocol (value = 0x00000002)
            const int SCARD_PROTOCOL_RAW = 4;// - Use with memory type cards (value = 0x00000004)
 
            IntPtr hCard = IntPtr.Zero;
            IntPtr activeProtocol = IntPtr.Zero;
            uint ret = SCardConnect(hContext, readerName, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T1, ref hCard, ref activeProtocol);
            if (ret != SCARD_S_SUCCESS)
            {
                throw new ApplicationException("カードに接続できません。code = " + ret);
            }
            return hCard;
 
        }
 
         void disconnect(IntPtr hCard) 
        {
            const int SCARD_LEAVE_CARD =     0; // Don't do anything special on close
            const int SCARD_RESET_CARD =     1; // Reset the card on close
            const int SCARD_UNPOWER_CARD =   2; // Power down the card on close
            const int SCARD_EJECT_CARD = 3; // Eject the card on close
 
            uint ret = SCardDisconnect(hCard, SCARD_LEAVE_CARD);
            if (ret != SCARD_S_SUCCESS)
            {
                throw new ApplicationException("カードとの接続を切断できません。code = " + ret);
            }
        }
 
        private IntPtr getPciT1()
        {
            IntPtr handle = LoadLibrary("Winscard.dll");
            IntPtr pci = GetProcAddress(handle, "g_rgSCardT1Pci");
            FreeLibrary(handle);
            return pci;
        }
 
         int transmit(IntPtr hCard, byte[] sendBuffer, byte[] recvBuffer)
        {
            SCARD_IO_REQUEST ioRecv = new SCARD_IO_REQUEST();
            ioRecv.cbPciLength = 255;
 
            int pcbRecvLength = recvBuffer.Length;
            int cbSendLength = sendBuffer.Length;
            IntPtr SCARD_PCI_T1 = getPciT1();
            uint ret = SCardTransmit(hCard, SCARD_PCI_T1, sendBuffer, cbSendLength, ioRecv, recvBuffer, ref pcbRecvLength);
            if (ret != SCARD_S_SUCCESS)
            {
                throw new ApplicationException("カードへの送信に失敗しました。code = " + ret);
            }
            return pcbRecvLength; // 受信したバイト数(recvBufferに受け取ったバイト数)
 
        }
 
         int control(IntPtr hCard, int controlCode, byte[] sendBuffer, byte[] recvBuffer)
        {
            int bytesReturned = 0;
            uint ret = SCardControl(hCard, controlCode, sendBuffer, sendBuffer.Length, recvBuffer, recvBuffer.Length, ref bytesReturned);
            if (ret != SCARD_S_SUCCESS)
            {
                throw new ApplicationException("カードへの制御命令送信に失敗しました。code = " + ret);
            }
            return bytesReturned;
        }
 
         SCARD_READERSTATE[] initializeReaderState(IntPtr hContext, List<string> readerNameList)
        {
            const int SCARD_STATE_UNAWARE = 0x00000000;
 
            SCARD_READERSTATE[] readerStateArray = new SCARD_READERSTATE[readerNameList.Count];
            int i = 0;
            foreach (string readerName in readerNameList)
            {
                readerStateArray[i].dwCurrentState = SCARD_STATE_UNAWARE;
                readerStateArray[i].szReader = readerName;
                i++;
            }
 
 
            uint ret = SCardGetStatusChange(hContext, 100/*msec*/, readerStateArray, readerStateArray.Length);
            if (ret != SCARD_S_SUCCESS)
            {
                throw new ApplicationException("リーダーの初期状態の取得に失敗。code = " + ret);
            }
            return readerStateArray;
        }
 
         void waitReaderStatusChange(IntPtr hContext, SCARD_READERSTATE[] readerStateArray, int timeoutMillis)
        {
            uint ret = SCardGetStatusChange(hContext, timeoutMillis/*msec*/, readerStateArray, readerStateArray.Length);
            switch(ret) {
                case SCARD_S_SUCCESS:
                    break;
                case SCARD_E_TIMEOUT:
                    throw new TimeoutException();
                default:
                    throw new ApplicationException("リーダーの状態変化の取得に失敗。code = " + ret);
            }
 
        }
 
         void updateCurrentState(SCARD_READERSTATE[] readerStateArray)
        {
            for (int i = 0; i < readerStateArray.Length; i++)
            {
                readerStateArray[i].dwCurrentState = readerStateArray[i].dwEventState;
            }
        }
 
 
 
    }
 
 
    class ReadResult
    {
        public string readerSerialNumber;
        public string cardId;
    }
 

 
}
