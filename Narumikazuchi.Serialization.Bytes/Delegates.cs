using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narumikazuchi.Serialization.Bytes
{
    public delegate Byte[] SerializationStrategy(Object toSerialize);

    public delegate Object DeserializationStrategy(Byte[] raw);
}
