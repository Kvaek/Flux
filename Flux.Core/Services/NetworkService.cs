using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;

namespace Flux.Core.Services {
	public class NetworkService : Service {
		public NetworkService() => Name = "Network";
		private IEventLoopGroup _bossGroup;
		private IEventLoopGroup _workerGroup;
		private IChannel _server;

		public override async Task Start() {
			try {
				_bossGroup = new MultithreadEventLoopGroup(1);
				_workerGroup = new MultithreadEventLoopGroup();

				ServerBootstrap bootstrap = new ServerBootstrap();
				bootstrap.Group(_bossGroup, _workerGroup);
				bootstrap.Channel<TcpServerSocketChannel>();

				bootstrap
					.Option(ChannelOption.SoBacklog, 128)
					.ChildOption(ChannelOption.TcpNodelay, true)
					.ChildOption(ChannelOption.SoKeepalive, true)
					.ChildHandler(new ActionChannelInitializer<IChannel>(channel => {
						IChannelPipeline pipeline = channel.Pipeline;
						pipeline.AddLast("read_timeout", new ReadTimeoutHandler(30));
						//pipeline.AddLast("frame_decoder", new FrameDecoder());
						//pipeline.AddLast("packet_decoder", new PacketDecoder());
						//pipeline.AddLast("frame_encoder", new FrameEncoder());
						//pipeline.AddLast("packet_encoder", new PacketEncoder());
						pipeline.AddLast("write_timeout", new WriteTimeoutHandler(30));
						//pipeline.AddLast("inbound_handler", new PacketInboundHandler());
					}));

				_server = await bootstrap.BindAsync(IPAddress.Any, Flux.INSTANCE.Cfg.Port).ConfigureAwait(false);
				Flux.INSTANCE.Logger.Info("Ready for connections...");
				Flux.INSTANCE.Logger.Info("To shutdown the server safely press CTRL+C");
				
				while (Running) await Task.Delay(1000).ConfigureAwait(false);
				await _server.CloseAsync().ConfigureAwait(false);
			}
			catch (Exception ex) {
				Flux.INSTANCE.Logger.Error("Bootstrap connection has been unexpectedly closed");
				throw new Exception("Bootstrap connection has been unexpectedly closed", ex);
			} finally {
				await _workerGroup.ShutdownGracefullyAsync();
				await _bossGroup.ShutdownGracefullyAsync();
			}
		}

		public override async Task Stop() {
			if (_server != null) {
				await _server.CloseAsync().ConfigureAwait(false);
			}
		}

		public override async Task Tick() {
			
		}
	}
}