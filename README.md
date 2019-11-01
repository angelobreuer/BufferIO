<!-- Banner -->
<a href="https://github.com/angelobreuer/BufferIO/">
	<img src="https://i.imgur.com/GJrvpca.png"/>
</a>

<!-- Center badges -->
<p align="center">
	
<!-- CodeFactor.io Badge -->
<a href="https://www.codefactor.io/repository/github/angelobreuer/BufferIO">
	<img alt="CodeFactor.io" src="https://www.codefactor.io/repository/github/angelobreuer/BufferIO/badge?style=for-the-badge" />	
</a>

<!-- Releases Badge -->
<a href="https://github.com/angelobreuer/BufferIO/releases">
	<img alt="GitHub tag (latest SemVer)" src="https://img.shields.io/github/tag/angelobreuer/BufferIO.svg?label=RELEASE&style=for-the-badge">
</a>

<!-- GitHub issues Badge -->
<a href="https://github.com/angelobreuer/BufferIO/issues">
	<img alt="GitHub issues" src="https://img.shields.io/github/issues/angelobreuer/BufferIO.svg?style=for-the-badge">	
</a>

<!-- AppVeyor CI (master) Badge -->
<a href="https://ci.appveyor.com/project/angelobreuer/BufferIO">
	<img alt="AppVeyor" src="https://img.shields.io/appveyor/ci/angelobreuer/BufferIO?style=for-the-badge">
</a>


</p>

[BufferIO](https://github.com/angelobreuer/BufferIO) is an easy-to-use big-endian oriented byte buffer.

### Features
- 🔌 **Easy Integration**
- ✳️ **Extensible**
- 📝 **Fully-documented**
- ⏱️ **Optimized for performance**
- ⬆ **Buffer Pooling**
- ⚡ **Endian conversion**
- 👾 **Support for [unsafe](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/unsafe) code**

<span>&nbsp;&nbsp;&nbsp;</span>*and a lot more...*

### Quick Start

Let's get started with an easy example:

##### Create / encode the data

```csharp
// Create the buffer
using var buffer = new ByteBuffer();
buffer.Write(Guid.NewGuid()); // Unique Key
buffer.Write("BufferIO"); // Product Name
buffer.Write(1568123183UL); // Product Release Date Time

// Send across the universe
socket.Send(buffer.GetBuffer());
```

##### Decode the data

```csharp
// Create the buffer
using var buffer = new ByteBuffer(data);
buffer.ReadGuid(); // -> Unique Key (...)
buffer.ReadString(); // -> Product Name ("BufferIO")
buffer.ReadULong(); // -> Product Release Date Time (1568123183)
```
