Install-TransportAgent -Name "ClamAgent" -TransportAgentFactory "ClamAgent.ClamAgentFactory" -AssemblyPath "C:\ExchAgent\ClamAgent.dll"
Enable-TransportAgent -Identity "ClamAgent"
net stop MSExchangeTransport
net start MSExchangeTransport