// Win32Project1.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "Win32Project1.h"

#include <math.h>

#include "libavutil/opt.h"
#include "libavcodec/avcodec.h"
#include "libavutil/channel_layout.h"
#include "libavutil/common.h"
#include "libavutil/imgutils.h"
#include "libavutil/mathematics.h"
#include "libavutil/samplefmt.h"

#define INBUF_SIZE 4096
#define AUDIO_INBUF_SIZE 20480
#define AUDIO_REFILL_THRESH 4096

// 这是导出函数的一个示例。
WIN32PROJECT1_API int fnWin32Project1(void)
{
	return 42;
}

/*
* Audio decoding.
*/
static void audio_decode(const char *outfilename, const char *filename)
{
	AVCodec *codec;
	AVCodecContext *c = NULL;
	int len;
	FILE *f, *outfile;
	uint8_t inbuf[AUDIO_INBUF_SIZE + FF_INPUT_BUFFER_PADDING_SIZE];
	AVPacket avpkt;
	AVFrame *decoded_frame = NULL;

	av_init_packet(&avpkt);

	printf("Decode audio file %s to %s\n", filename, outfilename);

	/* find the mpeg audio decoder */
	codec = avcodec_find_decoder(AV_CODEC_ID_MP2);
	if (!codec) {
		fprintf(stderr, "Codec not found\n");
		exit(1);
	}

	c = avcodec_alloc_context3(codec);
	if (!c) {
		fprintf(stderr, "Could not allocate audio codec context\n");
		exit(1);
	}

	/* open it */
	if (avcodec_open2(c, codec, NULL) < 0) {
		fprintf(stderr, "Could not open codec\n");
		exit(1);
	}

	f = fopen(filename, "rb");
	if (!f) {
		fprintf(stderr, "Could not open %s\n", filename);
		exit(1);
	}
	outfile = fopen(outfilename, "wb");
	if (!outfile) {
		av_free(c);
		exit(1);
	}

	/* decode until eof */
	avpkt.data = inbuf;
	avpkt.size = fread(inbuf, 1, AUDIO_INBUF_SIZE, f);

	while (avpkt.size > 0) {
		int got_frame = 0;

		if (!decoded_frame) {
			if (!(decoded_frame = av_frame_alloc())) {
				fprintf(stderr, "Could not allocate audio frame\n");
				exit(1);
			}
		}

		len = avcodec_decode_audio4(c, decoded_frame, &got_frame, &avpkt);
		if (len < 0) {
			fprintf(stderr, "Error while decoding\n");
			exit(1);
		}
		if (got_frame) {
			/* if a frame has been decoded, output it */
			int data_size = av_samples_get_buffer_size(NULL, c->channels,
				decoded_frame->nb_samples,
				c->sample_fmt, 1);
			if (data_size < 0) {
				/* This should not occur, checking just for paranoia */
				fprintf(stderr, "Failed to calculate data size\n");
				exit(1);
			}
			fwrite(decoded_frame->data[0], 1, data_size, outfile);
		}
		avpkt.size -= len;
		avpkt.data += len;
		avpkt.dts =
			avpkt.pts = AV_NOPTS_VALUE;
		if (avpkt.size < AUDIO_REFILL_THRESH) {
			/* Refill the input buffer, to avoid trying to decode
			* incomplete frames. Instead of this, one could also use
			* a parser, or use a proper container format through
			* libavformat. */
			memmove(inbuf, avpkt.data, avpkt.size);
			avpkt.data = inbuf;
			len = fread(avpkt.data + avpkt.size, 1,
				AUDIO_INBUF_SIZE - avpkt.size, f);
			if (len > 0)
				avpkt.size += len;
		}
	}

	fclose(outfile);
	fclose(f);

	avcodec_close(c);
	av_free(c);
	av_frame_free(&decoded_frame);
}
