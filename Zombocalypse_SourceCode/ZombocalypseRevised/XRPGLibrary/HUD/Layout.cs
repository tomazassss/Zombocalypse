using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.HUD
{
    public enum Style
    {
        ONE_COLUMN = 1,
        TWO_COLUMN = 2,
        THREE_COLUMN = 3
    }

    public class Layout
    {
        public static readonly Layout ONE_COLUMN_LAYOUT = new Layout(Style.ONE_COLUMN);

        private Style style;
        private float[] ratios;

        public Style Style
        {
            get { return style; }
            set { this.style = value; }
        }

        public float this[int i]
        {
            get { return ratios[i]; }
            set { this.ratios[i] = value; }
        }

        public Layout(Style style)
        {
            this.Style = style;
            float value = 1f / (float)style;
            this.ratios = new float[(int)style];
            for (int i = 0; i < this.ratios.Length; i++)
            {
                this.ratios[i] = value;
            }
        }

        public void SetRatios(params float[] args)
        {
            if (args.Length != (int)style - 1)
            {
                throw new Exception("Illegal arguments");
            }
            this.ratios = new float[args.Length + 1];

            float sum = 0;
            for (int i = 0; i < args.Length; i++)
            {
                sum += args[i];
                if (sum > 1)
                {
                    throw new Exception("Unreasonable ratios");
                }
                this.ratios[i] = args[i];
            }
            ratios[ratios.Length - 1] = 1 - sum;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (float ratio in ratios)
            {
                builder.Append(ratio + "\n");
            }
            return builder.ToString();
        }
    }

    
}
