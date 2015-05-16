Install-TransportAgent -Name "ClamAgent" -TransportAgentFactory "ClamAgent.ClamAgentFactory" -AssemblyPath "$args"
Enable-TransportAgent -Identity "ClamAgent"